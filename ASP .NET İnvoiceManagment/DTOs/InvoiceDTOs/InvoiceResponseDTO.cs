namespace ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;

/// <summary>
/// Data transfer object representing the summarized details of an invoice in responses.
/// </summary>
public class InvoiceResponseDTO
{
    /// <summary>
    /// The unique identifier of the invoice.
    /// </summary>
    /// <example>1</example>
    public Guid Id { get; set; }
    /// <summary>
    /// Key identifying the customer associated with this invoice.
    /// Should be a human-readable name or identifier.
    /// </summary>
    public string CustomerName { get; set; } = string.Empty;

    /// <summary>
    /// Comment or note associated with the invoice, providing additional context or information.
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// The start date of the period covered by the invoice.
    /// </summary>
    /// <example>2026-02-01T00:00:00Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// The end date of the period covered by the invoice.
    /// </summary>
    /// <example>2026-02-28T23:59:59Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// The total monetary value of the invoice, calculated from its rows.
    /// </summary>
    /// <example>1500.75</example>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// The current lifecycle status of the invoice.
    /// </summary>
    /// <example>Created</example>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// The number of line items (rows) included in this invoice.
    /// </summary>
    /// <example>3</example>
    public int InvoiceRowsCount { get; set; }
}