using ASP_.NET_InvoiceManagementAuth.DTOs;
using ASP_.NET_InvoiceManagementAuth.Models;
using FluentValidation;

namespace ASP_.NET_InvoiceManagementAuth.Validators;

/// <summary>
/// Validator for invoice creation requests, ensuring that mandatory fields, 
/// chronological date ranges, and lifecycle statuses are valid before processing.
/// </summary>
public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceRequest>
{
    /// <summary>
    /// Initializes strict validation rules for creating new invoices, 
    /// focusing on customer identification and logical date sequences.
    /// </summary>
    public CreateInvoiceValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("A valid Customer ID is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than the start date.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Comment));

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(BeAValidStatus).WithMessage("Invalid status value provided.");
    }

    /// <summary>
    /// Validates whether the provided string matches a defined value within the <see cref="InvoiceStatus"/> enumeration.
    /// </summary>
    /// <param name="status">The status string to verify.</param>
    /// <returns>True if the status is a valid member of the enum; otherwise, false.</returns>
    private bool BeAValidStatus(string? status)
    {
        return Enum.TryParse(typeof(InvoiceStatus), status, true, out _);
    }
}