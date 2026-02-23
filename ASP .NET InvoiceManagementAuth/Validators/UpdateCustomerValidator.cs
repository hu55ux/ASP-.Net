using ASP_.NET_InvoiceManagementAuth.DTOs.CustomerDTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagementAuth.Validators;

/// <summary>
/// Validator for partial customer updates, ensuring that at least one attribute is modified 
/// and that any provided data adheres to domain constraints.
/// </summary>
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    /// <summary>
    /// Initializes validation rules for optional customer fields, enforcing data integrity 
    /// only when a field is explicitly provided in the update request.
    /// </summary>
    public UpdateCustomerValidator()
    {
        // Global rule: Prevent empty update requests
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Name) ||
                       !string.IsNullOrWhiteSpace(x.Email) ||
                       !string.IsNullOrWhiteSpace(x.PhoneNumber) ||
                       !string.IsNullOrWhiteSpace(x.Address))
            .WithMessage("At least one field (Name, Email, PhoneNumber, or Address) must be provided for update.");

        // Conditional Name Validation
        RuleFor(x => x.Name)
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Name contains invalid characters.")
            .When(x => !string.IsNullOrEmpty(x.Name));

        // Conditional Email Validation
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(150).WithMessage("Email is too long.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        // Conditional Phone Number Validation
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid international phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        // Conditional Address Validation
        RuleFor(x => x.Address)
            .MinimumLength(10).WithMessage("Address is too short.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Address));
    }
}