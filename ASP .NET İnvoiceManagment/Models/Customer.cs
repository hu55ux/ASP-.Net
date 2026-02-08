namespace ASP_.NET_InvoiceManagment.Models;

/// <summary>
/// Represents a customer entity within the system.
/// </summary>
public class Customer
{
    /// <summary>
    /// Unique identifier for the customer.
    /// </summary>
    /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Full name or company name of the customer.
    /// </summary>
    /// <example>Nihad Mammadov</example>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Physical or billing address of the customer.
    /// </summary>
    /// <example>123 Baku Street, Azerbaijan</example>
    public string? Address { get; set; }

    /// <summary>
    /// Unique email address for communication.
    /// </summary>
    /// <example>nihad.m@example.com</example>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Contact phone number.
    /// </summary>
    /// <example>+994501234567</example>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// Record creation timestamp.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Last update timestamp.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; }

    /// <summary>
    /// Soft delete timestamp. If not null, the customer is considered archived.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }

    /// <summary>
    /// Navigation property for invoices associated with this customer.
    /// </summary>
    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
}