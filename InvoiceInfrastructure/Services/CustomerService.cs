using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.Database;
using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Models;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASP_.NET_InvoiceManagementAuth.Services;

/// <summary>
/// Service implementation for managing customer-related operations.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly InvoiceManagmentDbContext _dbContext;
    private readonly IMapper _mapper;

    /// <summary>
    /// Constructor for CustomerService, injecting the database context and AutoMapper instance.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="mapper"></param>
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

    /// <summary>
    /// Performs a paginated search for customers based on the provided query parameters.
    /// </summary>
    /// <param name="customerQuery"></param>
    /// <returns></returns>
    public async Task<PagedResult<CustomerResponseDTO>> GetPagedAsync(CustomerQueryDTO customerQuery)
    {
        var query = _dbContext.Customers
            .AsNoTracking()
            .Include(c => c.Invoices)
            .Where(c => c.DeletedAt == null)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(customerQuery.SearchTerm))
        {
            var searchTerm = customerQuery.SearchTerm.Trim().ToLower();

            query = query.Where(
                q => q.Name.ToLower().Contains(searchTerm) ||
                q.Address.ToLower().Contains(searchTerm) ||
                q.Email.ToLower().Contains(searchTerm));
        }

        if (!string.IsNullOrWhiteSpace(customerQuery.Sort))
        {
            query = ApplySorting(query, customerQuery.Sort, customerQuery.SortDirection);
        }
        else
        {
            query = query.OrderByDescending(c => c.Name);
        }
        var totalCount = await _dbContext.Customers.CountAsync();
        var skip = (customerQuery.Page - 1) * customerQuery.PageSize;
        var customers = await query
                                    .Skip(skip)
                                    .Take(customerQuery.PageSize)
                                    .ToListAsync();

        var customerDTO = _mapper.Map<IEnumerable<CustomerResponseDTO>>(customers);

        return PagedResult<CustomerResponseDTO>.Create(
            customerDTO,
            customerQuery.Page,
            customerQuery.PageSize,
            totalCount
            );
    }

    private IQueryable<Customer> ApplySorting(IQueryable<Customer> query, string sort, string sortDirection)
    {
        var isDescending = sortDirection?.ToLower() == "desc";
        return sort.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(c => c.Name) : query.OrderBy(c => c.Name),
            "email" => isDescending ? query.OrderByDescending(c => c.Email) : query.OrderBy(c => c.Email),
            "createdat" => isDescending ? query.OrderByDescending(c => c.CreatedAt) : query.OrderBy(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.Name)
        };
    }

    public async Task<Customer?> GetCustomerEntityAsync(Guid id, bool includeDeleted = false)
    {
        return await _dbContext.Customers
            .Include(c => c.Invoices)
            .FirstOrDefaultAsync(c => c.Id == id && (includeDeleted || c.DeletedAt == null));
    }
}