namespace ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;

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