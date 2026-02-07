using ASP_.NET_InvoiceManagment.Database;
using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using ASP_.NET_InvoiceManagment.Models;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_InvoiceManagment.Services;

/// <summary>
/// Service implementation for managing customer-related operations.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly InvoiceManagmentDbContext _dbContext;
    private readonly IMapper _mapper;

    public CustomerService(InvoiceManagmentDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    /// <summary>
    /// Archives a customer by setting a deletion timestamp (Soft Delete).
    /// </summary>
    /// <param name="id">The unique identifier of the customer.</param>
    /// <returns>True if the customer was successfully archived; otherwise, false.</returns>
    public async Task<bool> ArchiveCustomerAsync(Guid id)
    {
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == id && c.DeletedAt == null);

        if (customer is null) return false;

        customer.DeletedAt = DateTime.UtcNow;
        customer.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Creates a new customer record after validating email uniqueness.
    /// </summary>
    /// <param name="createCustomer">The customer creation request data.</param>
    /// <returns>A DTO representing the newly created customer.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the email is already in use.</exception>
    public async Task<CustomerResponseDTO> CreateAsync(CreateCustomerRequest createCustomer)
    {
        var exists = await _dbContext.Customers
            .AnyAsync(c => c.Email == createCustomer.Email && c.DeletedAt == null);

        if (exists)
            throw new InvalidOperationException($"Customer with Email: {createCustomer.Email} already exists!!");

        var customer = _mapper.Map<Customer>(createCustomer);

        _dbContext.Customers.Add(customer);
        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDTO>(customer);
    }

    /// <summary>
    /// Updates an existing customer's information.
    /// </summary>
    /// <param name="updateCustomer">The updated data.</param>
    /// <param name="id">The ID of the customer to update.</param>
    /// <returns>A DTO of the updated customer.</returns>
    public async Task<CustomerResponseDTO> UpdateAsync(UpdateCustomerRequest updateCustomer, Guid id)
    {
        // Optimization: No need to include Invoices for a simple profile update
        var updatingCustomer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Id == id);

        if (updatingCustomer is null)
            throw new KeyNotFoundException($"Customer with ID: {id} not found!!");

        _mapper.Map(updateCustomer, updatingCustomer);

        await _dbContext.SaveChangesAsync();

        return _mapper.Map<CustomerResponseDTO>(updatingCustomer);
    }

    /// <summary>
    /// Permanently deletes a customer if they have no associated invoices.
    /// </summary>
    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _dbContext.Customers
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (customer is null || customer.Invoices.Any())
            return false;

        _dbContext.Customers.Remove(customer);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Retrieves all active (non-archived) customers.
    /// </summary>
    public async Task<IEnumerable<CustomerResponseDTO>> GetAllAsync()
    {
        var customers = await _dbContext.Customers
            .AsNoTracking()
            .Include(c => c.Invoices)
            .Where(c => c.DeletedAt == null)
            .ToListAsync();

        return _mapper.Map<IEnumerable<CustomerResponseDTO>>(customers);
    }

    /// <summary>
    /// Retrieves a single customer by ID.
    /// </summary>
    /// <param name="id">Customer unique ID.</param>
    /// <param name="includeDeleted">Whether to search in archived records.</param>
    public async Task<CustomerResponseDTO?> GetByIdAsync(Guid id, bool includeDeleted = false)
    {
        var query = _dbContext.Customers.AsNoTracking().AsQueryable();

        if (!includeDeleted)
            query = query.Where(c => c.DeletedAt == null);

        var customer = await query.Include(c => c.Invoices).FirstOrDefaultAsync(c => c.Id == id);

        return customer == null ? null : _mapper.Map<CustomerResponseDTO>(customer);
    }

    /// <summary>
    /// Implementation of the HasInvoice check.
    /// </summary>
    /// <param name="customer">The customer entity to check.</param>
    /// <returns>True if invoices collection is not null and has items.</returns>
    public bool HasInvoice(Customer customer)
    {
        if (customer == null) return false;
        return customer.Invoices != null && customer.Invoices.Any();
    }
}