using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.DTOs.CustomerDTOs;
using ASP_.NET_InvoiceManagementAuth.Models;

namespace ASP_.NET_InvoiceManagementAuth.Services.Interfaces;

/// <summary>
/// Defines the contract for customer-related business operations.
/// </summary>
public interface ICustomerService
{
    /// <summary>
    /// Retrieves all active customers from the system.
    /// </summary>
    /// <returns>A collection of <see cref="CustomerResponseDTO"/>.</returns>
    Task<IEnumerable<CustomerResponseDTO>> GetAllAsync();

    /// <summary>
    /// Paginated retrieval of customers based on the provided query parameters,
    /// including filtering, sorting, and searching capabilities.
    /// </summary>
    /// <param name="customerQuery"></param>
    /// <returns> Contains a paginated list of <see cref="CustomerResponseDTO"/> matching the query criteria.</returns>
    Task<PagedResult<CustomerResponseDTO>> GetPagedAsync(CustomerQueryDTO customerQuery);

    /// <summary>
    /// Registers a new customer in the system.
    /// </summary>
    /// <param name="createCustomer">The data transfer object containing new customer details.</param>
    /// <returns>The created <see cref="CustomerResponseDTO"/>.</returns>
    Task<CustomerResponseDTO> CreateAsync(CreateCustomerRequest createCustomer);

    /// <summary>
    /// Updates the details of an existing customer.
    /// </summary>
    /// <param name="updateCustomer">The DTO containing updated customer information.</param>
    /// <param name="id">The unique identifier of the customer to update.</param>
    /// <returns>The updated <see cref="CustomerResponseDTO"/>.</returns>
    Task<CustomerResponseDTO> UpdateAsync(UpdateCustomerRequest updateCustomer, Guid id);

    /// <summary>
    /// Permanently removes a customer from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to delete.</param>
    /// <returns>True if deletion was successful; otherwise, false.</returns>
    Task<bool> DeleteAsync(Guid id);

    /// <summary>
    /// Soft deletes a customer by marking them as archived without removing the record.
    /// </summary>
    /// <param name="id">The unique identifier of the customer to archive.</param>
    /// <returns>True if archiving was successful; otherwise, false.</returns>
    Task<bool> ArchiveCustomerAsync(Guid id);

    /// <summary>
    /// Fetches a specific customer by their unique identifier.
    /// </summary>
    /// <param name="id">The customer ID.</param>
    /// <param name="includeDeleted">If set to true, includes archived (soft-deleted) customers in the search.</param>
    /// <returns>The <see cref="CustomerResponseDTO"/> if found; otherwise, null.</returns>
    Task<CustomerResponseDTO> GetByIdAsync(Guid id, bool includeDeleted = false);

    /// <summary>
    /// Checks whether the specified customer has any associated invoices.
    /// </summary>
    /// <param name="customer">The customer entity to check.</param>
    /// <returns>True if the customer has at least one invoice; otherwise, false.</returns>
    bool HasInvoice(Customer customer);
}