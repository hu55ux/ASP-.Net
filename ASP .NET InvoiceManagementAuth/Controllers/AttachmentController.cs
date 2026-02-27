using System.Security.Claims;
using ASP_.NET_InvoiceManagementAuth.Common;
using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Services;
using ASP_.NET_InvoiceManagementAuth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASP_.NET_InvoiceManagementAuth.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AttachmentController : ControllerBase
{
    private readonly IAttachmentService _attachmentService;
    private readonly I_InvoiceService _invoiceService;
    private readonly ICustomerService _customerService;
    private readonly IAuthorizationService _authorizationService;

    private string? UserId => User.FindFirstValue(ClaimTypes.NameIdentifier);

    public AttachmentController(
        IAttachmentService attachmentService,
        I_InvoiceService invoiceService,
        ICustomerService customerService,
        IAuthorizationService authorizationService)
    {
        _attachmentService = attachmentService;
        _invoiceService = invoiceService;
        _customerService = customerService;
        _authorizationService = authorizationService;
    }

    [HttpPost("~/api/invoices/{invoiceId}/attachments")]
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

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download(int id, CancellationToken cancellationToken)
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var info = await _attachmentService.GetAttachmentInfoAsync(id, cancellationToken);
        if (info is null)
            return NotFound();
        var invoice = await _invoiceService.GetInvoiceEntityAsync(info.InvoiceId);

        var deleted = await _attachmentService.DeleteAsync(id, cancellationToken);
        if (!deleted)
            return NotFound();
        return NoContent();
    }




}
