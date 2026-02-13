using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagment.Validators.CustomerValidators;

/// <summary>
/// Custom validator for validating the CreateCustomerRequest DTO using FluentValidation.
/// </summary>
public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    /// <summary>
    /// Constructor that defines the validation rules for creating a new customer.
    /// </summary>
    public CreateCustomerValidator()
    {
        // Name Validation
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Customer name is required.")
            .MinimumLength(2).WithMessage("Customer name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Customer name cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Customer name contains invalid characters.");

        // Email Validation
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("A valid email address is required.")
            .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.");

        // Phone Number Validation
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            // This Regex supports international formats like +994... or 050...
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is not in a valid international format.");

        // Address Validation
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MinimumLength(10).WithMessage("Please provide a more detailed address.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
    }
}