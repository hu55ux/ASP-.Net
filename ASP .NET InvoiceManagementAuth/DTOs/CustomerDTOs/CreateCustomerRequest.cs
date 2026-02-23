namespace ASP_.NET_InvoiceManagementAuth.DTOs.CustomerDTOs;

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