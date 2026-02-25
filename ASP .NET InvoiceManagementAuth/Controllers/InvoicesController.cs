using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_InvoiceManagementAuth.Controllers;

/// <summary>
/// API endpoints for managing invoices, including lifecycle operations and status changes.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class InvoicesController : ControllerBase
{
    private readonly I_InvoiceService _invoiceService;

    /// <summary>
    /// Constructor for InvoicesController, injecting the invoice service to handle business logic.
    /// </summary>
    /// <param name="invoiceService"></param>
    public InvoicesController(I_InvoiceService invoiceService)
    {
        _invoiceService = invoiceService;
    }

    /// <summary>
    /// Retrieves all non-deleted invoices from the system.
    /// </summary>
    /// <returns>A list of <see cref="InvoiceResponseDTO"/>.</returns>
    [HttpGet("All")]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceResponseDTO>>>> GetAll()
    {
        var invoices = await _invoiceService.GetAllAsync();
        if (invoices is null || !invoices.Any()) return NotFound(ApiResponse<object?>.ErrorResponse("Invoices not found!"));

        return Ok(ApiResponse<IEnumerable<InvoiceResponseDTO>>.SuccessResponse(invoices, "Invoices retrieved successfully!"));
    }

    /// <summary>
    /// Gets a paginated list of invoices based on query parameters for page number and page size.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<InvoiceResponseDTO>>>> GetPaged([FromQuery] InvoiceQueryDTO invoiceQuery)
    {
        var invoices = await _invoiceService.GetPagedAsync(invoiceQuery);
        if (invoices is null) return NotFound(ApiResponse<object?>.ErrorResponse("Invoices not found!"));

        return Ok(ApiResponse<PagedResult<InvoiceResponseDTO>>.SuccessResponse(invoices!, "Invoices retrieved successfully!"));
    }

    /// <summary>
    /// Retrieves a specific invoice by its unique GUID.
    /// 
    /// </summary>
    /// <param name="id">The unique identifier of the invoice.</param>
    /// <returns>The invoice details if found.</returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<InvoiceResponseDTO>> GetById(Guid id)
    {
        var invoice = await _invoiceService.GetByIdAsync(id);
        if (invoice is null) return NotFound($"Invoice with ID: {id} not found!!");
        return Ok(invoice);
    }

    /// <summary>
    /// Creates a new invoice based on the provided data.
    /// </summary>
    /// <param name="createInvoice">The creation request containing customer and period data.</param>
    /// <returns>The newly created invoice details.</returns>
    [HttpPost]
    public async Task<ActionResult<InvoiceResponseDTO>> Create([FromBody] CreateInvoiceRequest createInvoice)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        try
        {
            var createdInvoice = await _invoiceService.CreateAsync(createInvoice);
            return CreatedAtAction(nameof(GetById), new { id = createdInvoice.Id }, createdInvoice);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Soft deletes (archives) an invoice if it is not in 'Sent' status.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to archive.</param>
    [HttpPatch("{id:guid}/archive")]
    public async Task<ActionResult<bool>> Archive(Guid id)
    {
        var result = await _invoiceService.ArchiveInvoiceAsync(id);
        if (!result)
            return BadRequest(new { message = "Invoice not found or already archived/sent." });
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing invoice's header details.
    /// </summary>
    /// <param name="id">The GUID of the invoice to update.</param>
    /// <param name="updateInvoice">The updated information.</param>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult<InvoiceResponseDTO>> Update(Guid id, [FromBody] UpdateInvoiceRequest updateInvoice)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var updatedInvoice = await _invoiceService.UpdateAsync(updateInvoice, id);
            if (updatedInvoice is null) return NotFound($"Invoice with ID: {id} not found!!");
            return Ok(updatedInvoice);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Permanently deletes an invoice from the database.
    /// </summary>
    /// <param name="id">The unique identifier of the invoice to delete.</param>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var isDeleted = await _invoiceService.DeleteAsync(id);
        // FIX: Fixed typo 'İnvoice wiht' to 'Invoice with'
        if (!isDeleted) return NotFound($"Invoice with ID: {id} not found or cannot be deleted.");

        return NoContent();
    }
}