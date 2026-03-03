using ASP_.NET_InvoiceManagementAuth.DTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagementAuth.Validators;

/// <summary>
/// Validator for the customer creation request to ensure all business rules for customer data are met.
/// </summary>
public class CreateCustomerValidator : AbstractValidator<CreateCustomerRequest>
{
    /// <summary>
    /// Initializes validation rules for customer details including name format, email authenticity, and contact information.
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
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Phone number is not in a valid international format.");

        // Address Validation
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MinimumLength(10).WithMessage("Please provide a more detailed address.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");
    }
}