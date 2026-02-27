namespace ASP_.NET_InvoiceManagementAuth.DTOs;

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

/// <summary>
/// Query DTO for filtering and retrieving invoice data based on various
/// criteria such as date range, status, customer, etc.
/// </summary>
public class InvoiceQueryDTO
{
    /// <summary>
    /// Page number for pagination. Defaults to 1 if not provided.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Page size for pagination. Defaults to 10 if not provided.
    /// Maximum allowed is 100 to prevent performance issues.
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Search term for filtering invoices by customer name, 
    /// invoice comment, or other relevant fields.
    /// </summary>
    public string? SearchTerm { get; set; }
    /// <summary>
    /// Sorting field for ordering results. Common values include 
    /// "StartDate", "EndDate", "TotalSum", etc.
    /// </summary>
    public string Sort { get; set; } = string.Empty;
    /// <summary>
    /// Sorting direction for ordering results. Common values are "asc" for ascending
    /// desc for descending. Defaults to "asc" if not provided.
    /// </summary>
    public string SortDirection { get; set; } = string.Empty;
    /// <summary>
    /// Invoice status for filtering results. Common values include "Created", "Sent", "Paid", "Cancelled", etc.
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

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