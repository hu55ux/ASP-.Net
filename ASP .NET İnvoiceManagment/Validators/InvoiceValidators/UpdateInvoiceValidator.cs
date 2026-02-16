using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Models;
using FluentValidation;

namespace ASP_.NET_InvoiceManagment.Validators.InvoiceValidators;

/// <summary>
/// Validator for invoice updates, ensuring that at least one attribute is modified 
/// and that date ranges and status values remain consistent.
/// </summary>
public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceRequest>
{
    /// <summary>
    /// Initializes validation rules for invoice updates, enforcing consistency between 
    /// start and end dates and validating the invoice lifecycle status.
    /// </summary>
    public UpdateInvoiceValidator()
    {
        // Global rule: Prevent empty update requests
        RuleFor(x => x)
            .Must(x => x.CustomerId != default ||
                       x.StartDate != default ||
                       x.EndDate != default ||
                       !string.IsNullOrWhiteSpace(x.Comment) ||
                       !string.IsNullOrWhiteSpace(x.Status))
            .WithMessage("At least one field (CustomerId, StartDate, EndDate, Comment, or Status) must be provided for update.");

        // CustomerId Validation
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer identifier is required.")
            .When(x => x.CustomerId != default);

        // Date Validation
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.")
            .When(x => x.StartDate != default);

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than the start date.")
            .When(x => x.EndDate != default && x.StartDate != default);

        // Comment Validation
        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment length cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comment));

        // Status Validation
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(BeAValidStatus).WithMessage("Invalid status value. Allowed: Created, Sent, Paid, Cancelled.")
            .When(x => !string.IsNullOrEmpty(x.Status));
    }

    /// <summary>
    /// Checks if the provided status string can be parsed to a valid InvoiceStatus enum value.
    /// </summary>
    private bool BeAValidStatus(string? status)
    {
        if (string.IsNullOrWhiteSpace(status)) return false;

        return Enum.TryParse(typeof(InvoiceStatus), status, true, out _);
    }
}