using ASP_.NET_InvoiceManagment.Models;

namespace ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;

/// <summary>
/// Data transfer object for initiating a new invoice.
/// </summary>
public class CreateInvoiceRequest
{
    /// <summary>
    /// The unique identifier of the customer to whom the invoice belongs.
    /// </summary>
    /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The start date of the billing or service period.
    /// </summary>
    /// <example>2026-02-01T00:00:00Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// The end date of the billing or service period.
    /// </summary>
    /// <example>2026-02-28T23:59:59Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Optional remarks or notes regarding the invoice.
    /// </summary>
    /// <example>Monthly subscription fee for February 2026.</example>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// The initial lifecycle status of the invoice. 
    /// Typically defaults to 'Created' (0).
    /// </summary>
    /// <example>0</example>
    public string? Status { get; set; } = string.Empty;
}