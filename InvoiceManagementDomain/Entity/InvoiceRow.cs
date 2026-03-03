namespace ASP_.NET_InvoiceManagementAuth.Models;

/// <summary>
/// Represents an individual line item or service entry within an invoice.
/// </summary>
public class InvoiceRow
{
    /// <summary>
    /// Unique identifier for the invoice row.
    /// </summary>
    /// <example>b1c2d3e4-f5a6-b7c8-d9e0-f1a2b3c4d5e6</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier of the parent invoice this row belongs to.
    /// </summary>
    /// <example>a2b3c4d5-e6f7-g8h9-i0j1-k2l3m4n5o6p7</example>
    public Guid InvoiceId { get; set; }

    /// <summary>
    /// Description of the service or product provided.
    /// </summary>
    /// <example>Software Development Services</example>
    public string Service { get; set; } = string.Empty;

    /// <summary>
    /// The number of units or hours provided.
    /// </summary>
    /// <example>40.5</example>
    public decimal Quantity { get; set; }

    /// <summary>
    /// The cost per unit or hourly rate.
    /// </summary>
    /// <example>50.00</example>
    public decimal Amount { get; set; }

    /// <summary>
    /// Total calculated sum for this row (Quantity * Amount).
    /// This property is read-only.
    /// </summary>
    /// <example>2025.00</example>
    public decimal Sum
    {
        get => Quantity * Amount;
        set { } // Kept for serialization/compatibility, logic remains calculated
    }
}