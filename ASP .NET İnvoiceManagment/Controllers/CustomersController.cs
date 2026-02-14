using System.Threading.Tasks;
using ASP_.NET_InvoiceManagment.Common;
using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using ASP_.NET_InvoiceManagment.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
    /// ModelState errors to a dictionary of field names and their corresponding error messages.
    /// </summary>
    /// <param name="modelState"></param>
    /// <returns></returns>
    private static Dictionary<string, string[]> ToValidationErrors(ModelStateDictionary modelState)
    {
        return modelState
            .Where(x => x.Value?.Errors.Count > 0)
            .ToDictionary(
                k => k.Key,
                v => v.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
            );
    }

    /// <summary>
    /// Retrieves a list of all active customers.
    /// </summary>
    /// <returns>A list of <see cref="CustomerResponseDTO"/>.</returns>
    [HttpGet("All")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CustomerResponseDTO>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IEnumerable<CustomerResponseDTO>>>> GetAll()
    {
        var customers = await _customerService.GetAllAsync();
        if (customers is null || !customers.Any())
            return Ok(ApiResponse<IEnumerable<CustomerResponseDTO>>.SuccessResponse(customers, "No customers found!"));

        return Ok(ApiResponse<IEnumerable<CustomerResponseDTO>>.SuccessResponse(
            customers, "Customers returned successfully!"));
    }

    /// <summary>
    /// Gets a paginated list of customers based on the provided query parameters, 
    /// allowing for filtering and sorting.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CustomerResponseDTO>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<PagedResult<CustomerResponseDTO>>>> GetPaged([FromQuery] CustomerQueryDTO customerQuery)
    {
        var tasks = await _customerService.GetPagedAsync(customerQuery);

        return Ok(ApiResponse<PagedResult<CustomerResponseDTO>>.SuccessResponse(tasks, "Task items returned successfully"));
    }


    /// <summary>
    /// Retrieves a specific customer by their unique identifier.
    /// </summary>
    /// <param name="id">The GUID of the customer.</param>
    /// <returns>The customer details if found.</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> GetById(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
            return NotFound(ApiResponse<object>.ErrorResponse($"Customer with ID {id} not found"));

        return Ok(ApiResponse<CustomerResponseDTO>.SuccessResponse(customer, "Customer returned successfully!"));
    }

    /// <summary>
    /// Creates a new customer record.
    /// </summary>
    /// <param name="createCustomer">The customer creation request data.</param>
    /// <returns>The newly created customer details.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponseDTO>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> Create([FromBody] CreateCustomerRequest createCustomer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<Dictionary<string, string[]>>.ErrorResponse("Validation Error", ToValidationErrors(ModelState)));

        try
        {
            var createdCustomer = await _customerService.CreateAsync(createCustomer);
            return CreatedAtAction(
                nameof(GetById),
                new { id = createdCustomer.Id },
                ApiResponse<CustomerResponseDTO>.SuccessResponse(createdCustomer, "Customer created successfully!")
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Updates an existing customer's information.
    /// </summary>
    /// <param name="id">The GUID of the customer to update.</param>
    /// <param name="updateCustomer">The updated data.</param>
    /// <returns>The updated customer details if successful.</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<CustomerResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<Dictionary<string, string[]>>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<CustomerResponseDTO>>> Update(Guid id, [FromBody] UpdateCustomerRequest updateCustomer)
    {
        if (!ModelState.IsValid)
            return BadRequest(ApiResponse<Dictionary<string, string[]>>.ErrorResponse("Validation Error", ToValidationErrors(ModelState)));

        try
        {
            var updatedCustomer = await _customerService.UpdateAsync(updateCustomer, id);

            if (updatedCustomer is null)
                return NotFound(ApiResponse<object>.ErrorResponse($"Customer with ID: {id} not found!!"));

            return Ok(ApiResponse<CustomerResponseDTO>.SuccessResponse(updatedCustomer, "Customer updated successfully!!"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Archives a customer (Soft Delete) by their ID.
    /// </summary>
    /// <param name="id">The GUID of the customer to archive.</param>
    [HttpPatch("{id:guid}/archive")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<bool>>> Archive(Guid id)
    {
        var result = await _customerService.ArchiveCustomerAsync(id);
        if (!result)
            return BadRequest(ApiResponse<object>.ErrorResponse("Customer not found or already archived."));

        return Ok(ApiResponse<bool>.SuccessResponse(true, "Customer archived successfully."));
    }

    /// <summary>
    /// Permanently deletes a customer if they have no associated invoices.
    /// </summary>
    /// <param name="id">The GUID of the customer to delete.</param>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<object>>> Delete(Guid id)
    {
        var isDeleted = await _customerService.DeleteAsync(id);
        if (!isDeleted)
            return NotFound(ApiResponse<object>.ErrorResponse($"Customer with ID: {id} not found or has existing invoices!"));

        return Ok(ApiResponse<object>.SuccessResponse(null, "Customer deleted successfully!!"));
    }
}