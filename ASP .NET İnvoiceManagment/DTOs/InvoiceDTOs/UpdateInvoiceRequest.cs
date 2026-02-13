using ASP_.NET_InvoiceManagment.Models;

namespace ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;

/// <summary>
/// Data transfer object for updating an existing invoice's header information.
/// </summary>
public class UpdateInvoiceRequest
{
    /// <summary>
    /// The unique identifier of the customer. 
    /// Typically used for validation as changing the customer of an existing invoice is often restricted.
    /// </summary>
    /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// The updated start date for the invoice period.
    /// </summary>
    /// <example>2026-03-01T00:00:00Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// The updated end date for the invoice period.
    /// </summary>
    /// <example>2026-03-31T23:59:59Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Updated comments or remarks regarding the invoice.
    /// </summary>
    /// <example>Updated: Extended service period for March 2026.</example>
    public string? Comment { get; set; } = string.Empty;

    /// <summary>
    /// Status of the invoice, which may be updated to reflect changes 
    /// in the invoice's lifecycle (e.g., from "Created" to "Sent" or "Paid").
    /// </summary>
    /// <example>Sent</example>
    public string? Status { get; set; } = string.Empty;
}