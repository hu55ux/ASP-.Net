using Azure;

namespace ASP_.NET_InvoiceManagementAuth.DTOs.CustomerDTOs;

/// <summary>
/// Query DTO for Pagination, Sorting and Filtering of Customers
/// </summary>
public class CustomerQueryDTO
{
    /// <summary>
    /// Page number to retrieve (1-based index). Default is 1.
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Page size (number of records per page). Default is 10, maximum is 100.
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Filter by Customer Name , Address or Email  Case-insensitive partial match.
    /// </summary>
    public string? SearchTerm { get; set; }

    /// <summary>
    /// Sort by field: "Name", "Email", or "CreatedAt". Default is "Name"
    /// </summary>
    public string Sort { get; set; } = "Name";

    /// <summary>
    /// Sorting direction: "asc" or "desc". Default is "asc"
    /// </summary>
    public string SortDirection { get; set; } = "asc";

    /// <summary>
    /// Filter by Archive status. 
    /// If true, returns archived; if false, returns active; if null, returns all.
    /// </summary>
    public bool? IsArchived { get; set; } = false;
}