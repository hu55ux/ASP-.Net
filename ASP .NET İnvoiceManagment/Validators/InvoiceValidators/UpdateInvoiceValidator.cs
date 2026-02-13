using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Models;
using FluentValidation;

namespace ASP_.NET_InvoiceManagment.Validators.InvoiceValidators;

/// <summary>
/// Class for validating the UpdateInvoiceRequest DTO. This class uses FluentValidation
/// </summary>
public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceRequest>
{
    /// <summary>
    /// Constructor for Update Invoice Validator. Here we define all 
    /// the validation rules for the UpdateInvoiceRequest DTO.
    /// </summary>
    public UpdateInvoiceValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer identifier is required.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate)
            .WithMessage("End date must be later than the start date.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment length cannot exceed 500 characters.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required.")
            .Must(BeAValidStatus).WithMessage("Invalid status value. Allowed: Created, Sent, Paid, Cancelled.");
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