namespace ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;

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
