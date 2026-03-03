using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.Database;
using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Models;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Xceed.Document.NET;
using Xceed.Drawing;
using Xceed.Words.NET;
namespace ASP_.NET_InvoiceManagementAuth.Services;

/// <summary>
/// Service for managing invoice lifecycles, including creation, status updates, and calculations.
/// </summary>
public class InvoiceService : I_InvoiceService
{
    private readonly InvoiceManagmentDbContext _context;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for InvoiceService, injecting the database context and AutoMapper instance.ll
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public InvoiceService(InvoiceManagmentDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
    }

    /// <summary>
    /// Soft deletes (archives) an invoice if it has not been sent.
    /// </summary>
    /// <param name="id">Invoice unique identifier.</param>
    /// <returns>True if archived successfully, false if not found or already sent.</returns>
    public async Task<bool> ArchiveInvoiceAsync(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice is null || IsSent(invoice)) return false;

        invoice.DeletedAt = DateTimeOffset.UtcNow;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Checks if the invoice has progressed beyond the 'Created' state.
    /// Sent, Paid, or Cancelled invoices are usually locked for modification.
    /// </summary>
    public bool IsSent(Invoice invoice)
    {
        return invoice.Status != InvoiceStatus.Created;
    }

    /// <summary>
    /// Updates the status of an invoice based on business workflow rules.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="newStatus">The target status to transition to.</param>
    /// <returns>A mapped <see cref="InvoiceResponseDTO"/> of the updated invoice.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the status transition is logically invalid.</exception>
    public async Task<InvoiceResponseDTO?> ChangeStatusAsync(Guid id, InvoiceStatus newStatus)
    {
        // Retrieve the invoice ensuring it's not soft-deleted
        var invoice = await _context.Invoices
            .FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);

        if (invoice is null) return null;

        // Once an invoice is Paid or Cancelled, it becomes immutable to prevent financial inconsistencies.
        if (invoice.Status == InvoiceStatus.Paid || invoice.Status == InvoiceStatus.Cancelled)
        {
            throw new InvalidOperationException($"Transition from {invoice.Status} to {newStatus} is not allowed. Terminal states are immutable.");
        }

        // An invoice cannot be marked as Received or Paid if it hasn't been officially Sent yet.
        if (invoice.Status == InvoiceStatus.Created &&
           (newStatus == InvoiceStatus.Received || newStatus == InvoiceStatus.Paid))
        {
            throw new InvalidOperationException("Invoices must be marked as 'Sent' before they can transition to 'Received' or 'Paid'.");
        }

        // If the new status is the same as the current one, no database operation is needed.
        if (invoice.Status == newStatus)
            return _mapper.Map<InvoiceResponseDTO>(invoice);

        // Apply the update
        invoice.Status = newStatus;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;

        await _context.SaveChangesAsync();

        return _mapper.Map<InvoiceResponseDTO>(invoice);
    }

    /// <summary>
    /// Creates a new invoice and automatically calculates the total sum based on rows.
    /// </summary>
    public async Task<InvoiceResponseDTO> CreateAsync(CreateInvoiceRequest createInvoice)
    {
        var invoice = _mapper.Map<Invoice>(createInvoice);

        invoice.TotalSum = invoice.Rows?.Sum(r => r.Sum) ?? 0;

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        // Lazy loading alternative: Explicitly loading related data
        await _context.Entry(invoice).Reference(i => i.Customer).LoadAsync();
        await _context.Entry(invoice).Collection(i => i.Rows).LoadAsync();


        return _mapper.Map<InvoiceResponseDTO>(invoice);
    }

    /// <summary>
    /// Permanently deletes an invoice from the database if it hasn't been sent.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var invoice = await _context.Invoices.FindAsync(id);

        if (invoice is null || IsSent(invoice)) return false;

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Returns all non-deleted invoices with their customer and row details.
    /// </summary>
    public async Task<IEnumerable<InvoiceResponseDTO>> GetAllAsync()
    {
        var invoices = await _context.Invoices
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .Where(i => i.DeletedAt == null)
            .ToListAsync();

        return _mapper.Map<IEnumerable<InvoiceResponseDTO>>(invoices);
    }

    /// <summary>
    /// Retrieves a specific invoice by its ID.
    /// </summary>
    public async Task<InvoiceResponseDTO?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        var query = _context.Invoices
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .AsQueryable();

        if (!includeDeleted)
            query = query.Where(i => i.DeletedAt == null);

        var invoice = await query.FirstOrDefaultAsync(i => i.Id == id);

        return invoice is null ? null : _mapper.Map<InvoiceResponseDTO>(invoice);
    }

    /// <summary>
    /// Updates invoice details. Modifications are blocked if the invoice is already sent.
    /// </summary>
    public async Task<InvoiceResponseDTO> UpdateAsync(UpdateInvoiceRequest updateInvoice, Guid id)
    {
        var existingInvoice = await _context.Invoices
            .Include(i => i.Rows)
            .FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);

        if (existingInvoice is null)
            throw new KeyNotFoundException("Invoice not found or deleted");

        if (IsSent(existingInvoice))
            throw new InvalidOperationException("Changes cannot be made to a sent invoice.");

        _mapper.Map(updateInvoice, existingInvoice);

        // Recalculate total in case rows were modified
        existingInvoice.TotalSum = existingInvoice.Rows?.Sum(r => r.Sum) ?? 0;

        await _context.SaveChangesAsync();
        return _mapper.Map<InvoiceResponseDTO>(existingInvoice);
    }
    /// <summary>
    /// Gets a paginated list of invoices based on the provided query parameters, 
    /// including filtering, sorting, and searching capabilities.
    /// </summary>
    /// <param name="invoiceQuery"></param>
    /// <returns></returns>
    public async Task<PagedResult<InvoiceResponseDTO?>> GetPagedAsync(InvoiceQueryDTO invoiceQuery)
    {
        var query = _context.Invoices
            .AsNoTracking()
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .Where(i => i.DeletedAt == null);

        if (!string.IsNullOrWhiteSpace(invoiceQuery.SearchTerm))
        {
            query = query.Where(i =>
            i.Comment.Contains(invoiceQuery.SearchTerm));
        }

        if (!string.IsNullOrWhiteSpace(invoiceQuery.Status))
        {
            if (Enum.TryParse<InvoiceStatus>(invoiceQuery.Status, true, out var status))
            {
                query = query.Where(t => t.Status == status);
            }
        }


        if (!string.IsNullOrWhiteSpace(invoiceQuery.Sort))
        {
            query = ApplyFiltering(query, invoiceQuery.Sort, invoiceQuery.SortDirection);
        }
        else
        {
            query = query.OrderBy(i => i.CreatedAt);
        }

        var totalCount = await _context.Invoices.CountAsync(i => i.DeletedAt == null);
        var skip = (invoiceQuery.Page - 1) * invoiceQuery.PageSize;
        var invoices = await query
            .Skip(skip)
            .Take(invoiceQuery.PageSize)
            .ToListAsync();

        var invoiceDTOs = _mapper.Map<IEnumerable<InvoiceResponseDTO?>>(invoices);

        return PagedResult<InvoiceResponseDTO?>.Create(
                                                invoiceDTOs,
                                                invoiceQuery.Page,
                                                invoiceQuery.PageSize,
                                                totalCount);
    }

    private IQueryable<Invoice> ApplyFiltering(IQueryable<Invoice> query, string sort, string sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";
        return sort.ToLower() switch
        {
            "createdat" => isDescending ? query.OrderByDescending(i => i.CreatedAt) : query.OrderBy(i => i.CreatedAt),
            "totalsum" => isDescending ? query.OrderByDescending(i => i.TotalSum) : query.OrderBy(i => i.TotalSum),
            "status" => isDescending ? query.OrderByDescending(i => i.Status) : query.OrderBy(i => i.Status),
            _ => query.OrderBy(i => i.CreatedAt),
        };
    }

    public async Task<Invoice?> GetInvoiceEntityAsync(Guid id)
    {
        return await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);
    }

    public async Task<(byte[] Content, string FileName, string ContentType)?> ExportInvoiceAsync(Guid id, string format)
    {
        var invoice = _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Rows)
            .FirstOrDefault(i => i.Id == id && i.DeletedAt == null);

        if (invoice == null) return null;

        if (format?.ToLower() == "docx")
        {
            var content = GenerateDocX(invoice);
            return (content, $"Invoice_{invoice.Id}.docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
        }
        else
        {
            var content = GeneratePdf(invoice);
            return (content, $"Invoice_{invoice.Id}.pdf", "application/pdf");
        }
    }

    private byte[] GeneratePdf(Invoice invoice)
    {
        return QuestPDF.Fluent.Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Margin(50);

                // Header
                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text("INVOICE").FontSize(24).SemiBold().FontColor(Colors.Blue.Medium);
                        col.Item().Text($"ID: {invoice.Id}").FontSize(9).FontColor(Colors.Grey.Medium);
                    });

                    row.RelativeItem().AlignRight().Column(col =>
                    {
                        col.Item().Text($"Date: {invoice.CreatedAt:MMM dd, yyyy}").SemiBold();
                        col.Item().Text($"Period: {invoice.StartDate:dd/MM/yyyy} - {invoice.EndDate:dd/MM/yyyy}").FontSize(10);
                        col.Item().Text($"Status: {invoice.Status}").FontSize(10).Italic();
                    });
                });

                page.Content().PaddingVertical(20).Column(col =>
                {
                    // Customer Info
                    col.Item().PaddingBottom(15).Column(c =>
                    {
                        c.Item().Text("BILL TO:").FontSize(10).SemiBold().FontColor(Colors.Grey.Darken2);
                        c.Item().Text(invoice.Customer?.Name ?? "N/A").FontSize(14).Medium();
                    });

                    // Table
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(3); // Service Description
                            columns.RelativeColumn();  // Quantity
                            columns.RelativeColumn();  // Unit Price
                            columns.RelativeColumn();  // Total
                        });

                        table.Header(header =>
                        {
                            header.Cell().BorderBottom(1).Padding(5).Text("Service Description").SemiBold();
                            header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Qty").SemiBold();
                            header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Price").SemiBold();
                            header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Total").SemiBold();
                        });

                        foreach (var row in invoice.Rows)
                        {
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).Text(row.Service);
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text(row.Quantity.ToString("N2"));
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text($"{row.Amount:N2}");
                            table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten3).Padding(5).AlignRight().Text($"{row.Sum:N2}");
                        }
                    });

                    // Calculations
                    col.Item().AlignRight().PaddingTop(10).Column(c =>
                    {
                        c.Item().Text($"Total Sum: {invoice.TotalSum:N2} USD").FontSize(16).Bold().FontColor(Colors.Blue.Medium);
                    });

                    // Comments
                    if (!string.IsNullOrWhiteSpace(invoice.Comment))
                    {
                        col.Item().PaddingTop(30).Column(c =>
                        {
                            c.Item().Text("Notes:").FontSize(10).SemiBold();
                            c.Item().Text(invoice.Comment).FontSize(10).Italic();
                        });
                    }
                });

                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Page ");
                    x.CurrentPageNumber();
                });
            });
        }).GeneratePdf();
    }

    private byte[] GenerateDocX(Invoice invoice)
    {
        using var stream = new MemoryStream();
        using (var doc = DocX.Create(stream))
        {
            // Title
            var title = doc.InsertParagraph("INVOICE")
                .Bold()
                .FontSize(22)
                .Color(Color.DarkBlue);
            title.Alignment = Alignment.center;

            doc.InsertParagraph($"Invoice ID: {invoice.Id}").FontSize(9).Italic().Alignment = Alignment.center;
            doc.InsertParagraph().SpacingAfter(20);

            // Details
            doc.InsertParagraph($"Customer: {invoice.Customer?.Name ?? "N/A"}").Bold();
            doc.InsertParagraph($"Date: {invoice.CreatedAt:MMM dd, yyyy}");
            doc.InsertParagraph($"Billing Period: {invoice.StartDate:dd/MM/yyyy} - {invoice.EndDate:dd/MM/yyyy}");
            doc.InsertParagraph($"Status: {invoice.Status}");
            doc.InsertParagraph().SpacingAfter(15);

            // Table
            var rowsList = invoice.Rows.ToList();
            var table = doc.AddTable(rowsList.Count + 1, 4);
            table.Design = TableDesign.TableGrid;
            table.Alignment = Alignment.center;

            // Headers
            table.Rows[0].Cells[0].Paragraphs[0].Append("Service Description").Bold();
            table.Rows[0].Cells[1].Paragraphs[0].Append("Qty").Bold();
            table.Rows[0].Cells[2].Paragraphs[0].Append("Unit Price").Bold();
            table.Rows[0].Cells[3].Paragraphs[0].Append("Total").Bold();

            // Rows
            for (int i = 0; i < rowsList.Count; i++)
            {
                var rowData = rowsList[i];
                table.Rows[i + 1].Cells[0].Paragraphs[0].Append(rowData.Service);
                table.Rows[i + 1].Cells[1].Paragraphs[0].Append(rowData.Quantity.ToString("N2"));
                table.Rows[i + 1].Cells[2].Paragraphs[0].Append($"{rowData.Amount:N2}");
                table.Rows[i + 1].Cells[3].Paragraphs[0].Append($"{rowData.Sum:N2}");
            }

            doc.InsertTable(table);

            // Grand Total
            doc.InsertParagraph().SpacingBefore(15);
            var totalPara = doc.InsertParagraph($"GRAND TOTAL: {invoice.TotalSum:N2} USD")
                .Bold().FontSize(14);
            totalPara.Alignment = Alignment.right;

            // Remarks
            if (!string.IsNullOrWhiteSpace(invoice.Comment))
            {
                doc.InsertParagraph().SpacingBefore(20);
                doc.InsertParagraph("Notes:").Bold();
                doc.InsertParagraph(invoice.Comment).Italic();
            }

            doc.Save();
        }
        return stream.ToArray();
    }

}