using System.Security.Claims;
using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Services;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_InvoiceManagementAuth.Controllers;

/// <summary>
/// Provides endpoints for managing file attachments associated with invoices.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;
    private readonly I_InvoiceService _invoiceService;
    private readonly ICustomerService _customerService;

    /// <summary>
    /// Gets the unique identifier of the currently authenticated user.
    /// </summary>
    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);


    /// <summary>
    /// Initializes a new instance of the <see cref="AttachmentController"/> class.
    /// </summary>
    /// <param name="attachmentService">Service for managing file uploads, downloads, and deletions.</param>
    /// <param name="invoiceService">Service for accessing and validating invoice data.</param>
    /// <param name="customerService">Service for verifying customer ownership and details.</param>
    public AttachmentController(
        IAttachmentService attachmentService,
        I_InvoiceService invoiceService,
        ICustomerService customerService)
    {
        _attachmentService = attachmentService;
        _invoiceService = invoiceService;
        _customerService = customerService;
    }

    /// <summary>
    /// Uploads a file and attaches it to a specific invoice.
    /// </summary>
    /// <param name="invoiceId">The unique identifier (GUID) of the invoice.</param>
    /// <param name="file">The file to be uploaded from the multi-part form data.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>A success response containing the metadata of the uploaded attachment.</returns>
    /// <response code="200">Returns the created attachment metadata.</response>
    /// <response code="400">If the file is missing or empty.</response>
    /// <response code="404">If the invoice or associated customer cannot be found.</response>
    [HttpPost("~/api/invoices/{invoiceId}/attachments")]
    [ProducesResponseType(typeof(ApiResponse<AttachmentResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AttachmentResponseDto>>> Upload(
        Guid invoiceId,
        IFormFile file,
        CancellationToken cancellationToken)
    {
        var invoice = await _invoiceService.GetInvoiceEntityAsync(invoiceId);
        if (invoice is null)
            return NotFound();

        var customer = await _customerService.GetCustomerEntityAsync(invoice.CustomerId);
        if (customer is null)
            return NotFound();

        if (file is null || file.Length == 0)
            return BadRequest(ApiResponse<AttachmentResponseDto>.ErrorResponse("File is required"));

        await using var stream = file.OpenReadStream();
        var attachment = await _attachmentService.UploadAsync(
            invoiceId,
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            UserId!,
            cancellationToken);

        if (attachment is null)
            return NotFound();

        return Ok(ApiResponse<AttachmentResponseDto>.SuccessResponse(attachment, "File uploaded successfully"));
    }

    /// <summary>
    /// Downloads an attachment by its unique ID.
    /// </summary>
    /// <param name="id">The Guid ID of the attachment.</param>
    /// <param name="cancellationToken">Token to cancel the request.</param>
    /// <returns>The file stream with the appropriate content type and filename.</returns>
    /// <response code="200">Returns the file for download.</response>
    /// <response code="404">If the attachment, invoice, or file data is not found.</response>
    [HttpGet("{id}/download")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
    {
        var info = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (info is null)
            return NotFound();

        var invoice = await _invoiceService.GetInvoiceEntityAsync(info.InvoiceId);
        if (invoice is null)
            return NotFound();

        var customer = await _customerService.GetCustomerEntityAsync(invoice.CustomerId);
        if (customer is null)
            return NotFound();

        var result = await _attachmentService.GetDownloadAsync(id, cancellationToken);
        if (result is null)
            return NotFound();

        return File(result.Value.stream, result.Value.contentType, result.Value.fileName);
    }

    /// <summary>
    /// Deletes a specific attachment by its unique identifier.
    /// </summary>
    /// <param name="id">The unique integer identifier of the attachment.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="NoContentResult"/> if the deletion is successful.</returns>
    /// <response code="204">The attachment was successfully deleted.</response>
    /// <response code="404">The attachment or its associated invoice could not be found.</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var info = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (info is null)
            return NotFound();

        var invoice = await _invoiceService.GetInvoiceEntityAsync(info.InvoiceId);
        if (invoice is null)
            return NotFound();

        var deleted = await _attachmentService.DeleteAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();

        return NoContent();
    }
}