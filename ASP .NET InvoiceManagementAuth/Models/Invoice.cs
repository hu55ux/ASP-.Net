namespace ASP_.NET_InvoiceManagementAuth.Models;

/// <summary>
/// Represents a billing invoice containing multiple items and customer association.
/// </summary>
public class Invoice
{
    /// <summary>
    /// Unique identifier for the invoice.
    /// </summary>
    /// <example>a2b3c4d5-e6f7-g8h9-i0j1-k2l3m4n5o6p7</example>
    public Guid Id { get; set; }

    /// <summary>
    /// Identifier of the customer associated with this invoice.
    /// </summary>
    /// <example>f47ac10b-58cc-4372-a567-0e02b2c3d479</example>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Navigation property for the associated customer.
    /// </summary>
    public virtual Customer Customer { get; set; } = new Customer();

    /// <summary>
    /// The starting date of the billing period or service.
    /// </summary>
    /// <example>2024-01-01T00:00:00Z</example>
    public DateTimeOffset StartDate { get; set; }

    /// <summary>
    /// The ending date of the billing period or service.
    /// </summary>
    /// <example>2024-01-31T23:59:59Z</example>
    public DateTimeOffset EndDate { get; set; }

    /// <summary>
    /// Collection of individual line items within the invoice.
    /// </summary>
    public IEnumerable<InvoiceRow> Rows { get; set; } = new List<InvoiceRow>();

    /// <summary>
    /// Total calculated amount for all invoice rows.
    /// </summary>
    /// <example>1250.50</example>
    public decimal TotalSum { get; set; }

    /// <summary>
    /// Additional notes or internal remarks about the invoice.
    /// </summary>
    /// <example>Standard monthly service fee for January.</example>
    public string? Comment { get; set; }

    /// <summary>
    /// Current lifecycle status of the invoice.
    /// </summary>
    /// <example>0</example> // Created
    public InvoiceStatus Status { get; set; } = Models.InvoiceStatus.Created;

    /// <summary>
    /// Timestamp when the invoice was generated.
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// Timestamp of the last modification.
    /// </summary>
    public DateTimeOffset? UpdatedAt { get; set; } // 'UpdaatedAt' typo düzəldildi

    /// <summary>
    /// Timestamp for soft deletion. If populated, the invoice is considered archived.
    /// </summary>
    public DateTimeOffset? DeletedAt { get; set; }
    /// <summary>
    /// Collection of supporting documents or files associated with this invoice.
    /// </summary>
    public virtual ICollection<InvoiceAttachment> Attachments { get; set; } = new List<InvoiceAttachment>();
}

/// <summary>
/// Defines the various states an invoice can transition through.
/// </summary>
public enum InvoiceStatus
{
    /// <summary> Initial state after creation. </summary>
    Created,
    /// <summary> Invoice has been dispatched to the customer. </summary>
    Sent,
    /// <summary> Customer has confirmed receipt of the invoice. </summary>
    Received,
    /// <summary> Payment has been successfully processed. </summary>
    Paid,
    /// <summary> Invoice has been invalidated by the issuer. </summary>
    Cancelled,
    /// <summary> Customer has declined or disputed the invoice. </summary>
    Rejected
}