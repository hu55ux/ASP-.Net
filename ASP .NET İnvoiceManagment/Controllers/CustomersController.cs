using ASP_.NET_InvoiceManagment.Common;
using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_InvoiceManagment.Controllers;

/// <summary>
/// Provides endpoints for managing customers, including retrieval, creation, updates, and deletion.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomersController"/> class with the specified customer service.
    /// </summary>
    private readonly ICustomerService _customerService;
    /// <summary>
    /// Constructor for CustomersController, initializes the customer service dependency.
    /// </summary>
    /// <param name="customerService"></param>
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    /// <summary>
    /// Retrieves a list of all active customers.
    /// </summary>
    /// <returns>A list of <see cref="CustomerResponseDTO"/>.</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDTO>>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<CustomerResponseDTO>>.SuccessResponse(
            customers, "Customers returned successfully!"));
    }

    /// <summary>
    /// Retrieves a specific customer by their unique identifier.
    /// </summary>
    /// <param name="id">The GUID of the customer.</param>
    /// <returns>The customer details if found.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer is null) return NotFound($"Customer with ID: {id} not found!!");
        return Ok(customer);
    }

    /// <summary>
    /// Creates a new customer record.
    /// </summary>
    /// <param name="createCustomer">The customer creation request data.</param>
    /// <returns>The newly created customer details.</returns>
    [HttpPost]
    public async Task<ActionResult<CustomerResponseDTO>> Create([FromBody] CreateCustomerRequest createCustomer)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var createdCustomer = await _customerService.CreateAsync(createCustomer);

            // FIX: Added ID to route values (assuming Name is returned in DTO, adjust if DTO has Id)
            // If your CustomerResponseDTO doesn't have ID, you might need to add it or use another field.
            return CreatedAtAction(nameof(GetById), new { id = createdCustomer.Id }, createdCustomer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing customer's information.
    /// </summary>
    /// <param name="id">The GUID of the customer to update.</param>
    /// <param name="updateCustomer">The updated data.</param>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<CustomerResponseDTO>> Update(Guid id, [FromBody] UpdateCustomerRequest updateCustomer)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            // FIX: Changed parameter order to match Service definition (UpdateAsync(updateCustomer, id))
            var updatedCustomer = await _customerService.UpdateAsync(updateCustomer, id);

            if (updatedCustomer is null) return NotFound($"Customer with ID: {id} not found!!");
            return Ok(updatedCustomer);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Archives a customer (Soft Delete) by their ID.
    /// </summary>
    /// <param name="id">The GUID of the customer to archive.</param>
    [HttpPatch("{id:guid}/archive")]
    public async Task<ActionResult<bool>> Archive(Guid id) // FIX: Removed [FromBody] as ID comes from URL
    {
        var result = await _customerService.ArchiveCustomerAsync(id);
        if (!result)
            return BadRequest(new { message = "Customer not found or already archived." });
        return Ok(result);
    }

    /// <summary>
    /// Permanently deletes a customer if they have no associated invoices.
    /// </summary>
    /// <param name="id">The GUID of the customer to delete.</param>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var isDeleted = await _customerService.DeleteAsync(id);
        if (!isDeleted) return NotFound($"Customer with ID: {id} not found or has existing invoices!");
        return NoContent();
    }
}