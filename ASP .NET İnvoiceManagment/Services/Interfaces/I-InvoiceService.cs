using ASP_.NET_InvoiceManagment.Common;
using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Models;

namespace ASP_.NET_InvoiceManagment.Services.Interfaces;

/// <summary>
/// Defines the contract for business operations related to invoice management.
/// </summary>
public interface I_InvoiceService
{
    /// <summary>
    /// Creates a new invoice in the system.
    /// </summary>
    /// <param name="invoice">The request DTO containing invoice and item details.</param>
    /// <returns>A <see cref="InvoiceResponseDTO"/> representing the created invoice.</returns>
    Task<InvoiceResponseDTO> CreateAsync(CreateInvoiceRequest invoice);

    /// <summary>
    /// Updates an existing invoice's details. 
    /// Note: Implementation should prevent updates if the invoice is already sent or paid.
    /// </summary>
    /// <param name="invoiceUpdate">The updated data for the invoice.</param>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <returns>The updated <see cref="InvoiceResponseDTO"/>.</returns>
    Task<InvoiceResponseDTO> UpdateAsync(UpdateInvoiceRequest invoiceUpdate, Guid id);

    /// <summary>
    /// Permanently removes an invoice record from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <returns>True if the invoice was deleted; otherwise, false.</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Retrieves a specific invoice by its identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="includeDeleted">If true, includes archived/soft-deleted invoices in the result.</param>
    /// <returns>The <see cref="InvoiceResponseDTO"/> if found; otherwise, null.</returns>
    Task<InvoiceResponseDTO?> GetByIdAsync(Guid id, bool includeDeleted = false);

    /// <summary>
    /// Retrieves all active (non-archived) invoices.
    /// </summary>
    /// <returns>A collection of <see cref="InvoiceResponseDTO"/>.</returns>
    Task<IEnumerable<InvoiceResponseDTO>> GetAllAsync();
    /// <summary>
    /// Gets a paginated list of invoices based on the provided query parameters, 
    /// including filtering, sorting, and searching capabilities.
    /// </summary>
    /// <param name="ınvoiceQuery"></param>
    /// <returns></returns>
    Task<PagedResult<InvoiceResponseDTO?>> GetPagedAsync(InvoiceQueryDTO ınvoiceQuery);

    /// <summary>
    /// Updates the lifecycle status of an invoice (e.g., from Created to Sent).
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <param name="status">The new <see cref="InvoiceStatus"/> to apply.</param>
    /// <returns>The updated <see cref="InvoiceResponseDTO"/>.</returns>
    Task<InvoiceResponseDTO?> ChangeStatusAsync(Guid id, InvoiceStatus status);

    /// <summary>
    /// Marks an invoice as archived (soft-delete) by setting the deletion timestamp.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <returns>True if archived successfully; otherwise, false.</returns>
    Task<bool> ArchiveInvoiceAsync(Guid id);

    /// <summary>
    /// Determines if the invoice has already been sent to the customer.
    /// Typically used to lock an invoice from further modifications.
    /// </summary>
    /// <param name="invoice">The invoice entity to check.</param>
    /// <returns>True if the status is not 'Created'; otherwise, false.</returns>
    bool IsSent(Invoice invoice);
}