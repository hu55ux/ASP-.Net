using ASP_.NET_InvoiceManagment.Database;
using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Models;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_InvoiceManagment.Services;

/// <summary>
/// Service for managing invoice lifecycles, including creation, status updates, and calculations.
/// </summary>
public class InvoiceService : I_InvoiceService
{
    private readonly InvoiceManagmentDbContext _context;

    public InvoiceService(InvoiceManagmentDbContext context)
    {
        _context = context;
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
    /// Updates the current status of an invoice.
    /// </summary>
    public async Task<InvoiceResponseDTO?> ChangeStatusAsync(Guid id, InvoiceStatus status)
    {
        var invoice = await _context.Invoices
         .FirstOrDefaultAsync(i => i.Id == id && i.DeletedAt == null);

        if (invoice is null) return null;

        invoice.Status = status;
        invoice.UpdatedAt = DateTimeOffset.UtcNow;
        await _context.SaveChangesAsync();

        return MapToInvoiceDTO(invoice);
    }

    /// <summary>
    /// Creates a new invoice and automatically calculates the total sum based on rows.
    /// </summary>
    public async Task<InvoiceResponseDTO> CreateAsync(CreateInvoiceRequest createInvoice)
    {
        var invoice = new Invoice
        {
            CustomerId = createInvoice.CustomerId,
            StartDate = createInvoice.StartDate,
            EndDate = createInvoice.EndDate,
            Comment = createInvoice.Comment,
            Status = createInvoice.Status,
            CreatedAt = DateTimeOffset.UtcNow,
            TotalSum = 0 // Initially 0, will be updated if rows exist
        };

        invoice.TotalSum = invoice.Rows?.Sum(r => r.Sum) ?? 0;

        _context.Invoices.Add(invoice);
        await _context.SaveChangesAsync();

        // Lazy loading alternative: Explicitly loading related data
        await _context.Entry(invoice).Reference(i => i.Customer).LoadAsync();
        await _context.Entry(invoice).Collection(i => i.Rows).LoadAsync();

        return MapToInvoiceDTO(invoice);
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

        return invoices.Select(MapToInvoiceDTO);
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

        return invoice is null ? null : MapToInvoiceDTO(invoice);
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

        existingInvoice.StartDate = updateInvoice.StartDate;
        existingInvoice.EndDate = updateInvoice.EndDate;
        existingInvoice.Comment = updateInvoice.Comment;
        existingInvoice.UpdatedAt = DateTimeOffset.UtcNow;

        // Recalculate total in case rows were modified
        existingInvoice.TotalSum = existingInvoice.Rows?.Sum(r => r.Sum) ?? 0;

        await _context.SaveChangesAsync();
        return MapToInvoiceDTO(existingInvoice);
    }

    /// <summary>
    /// Maps Invoice entity to InvoiceResponseDTO.
    /// </summary>
    private InvoiceResponseDTO MapToInvoiceDTO(Invoice invoice)
    {
        return new InvoiceResponseDTO
        {
            // Assuming Id and other fields exist in your DTO
            StartDate = invoice.StartDate,
            EndDate = invoice.EndDate,
            TotalSum = invoice.TotalSum,
            Status = invoice.Status.ToString(),
            InvoiceCount = invoice.Rows?.Count ?? 0
        };
    }
}