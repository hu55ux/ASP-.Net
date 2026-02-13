using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using ASP_.NET_InvoiceManagment.Models;
using FluentValidation;

namespace ASP_.NET_InvoiceManagment.Validators.InvoiceValidators;

/// <summary>
/// Class for validating the CreateInvoiceRequest DTO. This class uses FluentValidation to define rules for the 
/// properties of CreateInvoiceRequest, ensuring that the data provided 
/// when creating a new invoice is valid and meets the necessary criteria.
/// </summary>
public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceRequest>
{
    /// <summary>
    /// Constructor for CreateInvoiceValidator. Here we define all 
    /// the validation rules for the CreateInvoiceRequest DTO.
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
    /// Status checking for enum type. If the Status property in CreateInvoiceRequest is 
    /// of type InvoiceStatus, this method checks if the provided status is a valid enum value.
    /// </summary>
    private bool BeAValidStatus(string? status)
    {
        return Enum.TryParse(typeof(InvoiceStatus), status, true, out _);
    }

}