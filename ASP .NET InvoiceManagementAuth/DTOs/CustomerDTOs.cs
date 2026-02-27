namespace ASP_.NET_InvoiceManagementAuth.DTOs;

/// <summary>
/// Data transfer object for creating a new customer record.
/// </summary>
public class CreateCustomerRequest
{
    /// <summary>
    /// The full name or legal business name of the customer.
    /// </summary>
    /// <example>Nihad Mammadov</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The primary email address for the customer. Used for login or communication.
    /// </summary>
    /// <example>nihad.m@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone number of the customer.
    /// </summary>
    /// <example>+994501234567</example>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// Registered physical or billing address.
    /// </summary>
    /// <example>123 Baku Street, Azerbaijan</example>
    public string Address { get; set; } = string.Empty;
}

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

/// <summary>
/// Data transfer object representing a customer's details in API responses.
/// </summary>
public class CustomerResponseDTO
{
    /// <summary>
    /// The unique identifier for the customer.
    /// </summary>
    /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
    public Guid Id { get; set; }

    /// <summary>
    /// The full name of the customer.
    /// </summary>
    /// <example>Nihad Mammadov</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The registered email address of the customer.
    /// </summary>
    /// <example>nihad.m@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The contact phone number of the customer.
    /// </summary>
    /// <example>+994501234567</example>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// The physical or billing address provided by the customer.
    /// </summary>
    /// <example>123 Baku Street, Azerbaijan</example>
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// The total number of invoices associated with this customer.
    /// </summary>
    /// <example>5</example>
    public int InvoiceCount { get; set; }

    /// <summary>
    /// Indicates whether the customer has at least one invoice in the system.
    /// Useful for UI logic to enable/disable specific actions.
    /// </summary>
    /// <example>true</example>
    public bool HasInvoices { get; set; }
}

/// <summary>
/// Data transfer object for updating existing customer information.
/// </summary>
public class UpdateCustomerRequest
{
    /// <summary>
    /// The updated full name or business name of the customer.
    /// </summary>
    /// <example>Nihad Mammadov</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The updated email address. Note: Changing this may affect authentication if linked.
    /// </summary>
    /// <example>nihad.m.new@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The updated contact phone number.
    /// </summary>
    /// <example>+994509876543</example>
    public string PhoneNumber { get; set; } = string.Empty;

    /// <summary>
    /// The updated physical or billing address.
    /// </summary>
    /// <example>456 Sumqayit Ave, Azerbaijan</example>
    public string Address { get; set; } = string.Empty;
}