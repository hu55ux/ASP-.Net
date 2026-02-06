namespace ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;

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