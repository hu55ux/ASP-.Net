using ASP_.NET_InvoiceManagment.DTOs.CustomerDTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagment.Validators.CustomerValidators;

/// <summary>
/// Validator for UpdateCustomerRequest, ensuring that at least one field is 
/// provided for update and that any provided fields meet specific validation criteria.
/// </summary>
public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerRequest>
{
    /// <summary>
    /// Constructor that defines validation rules for updating a customer. It ensures that at
    /// least one field is provided and applies specific rules to each field if they are not empty.
    /// </summary>
    public UpdateCustomerValidator()
    {
        // 1. Ümumi yoxlama: Ən azı bir sahə dolu olmalıdır
        RuleFor(x => x)
            .Must(x => !string.IsNullOrWhiteSpace(x.Name) ||
                       !string.IsNullOrWhiteSpace(x.Email) ||
                       !string.IsNullOrWhiteSpace(x.PhoneNumber) ||
                       !string.IsNullOrWhiteSpace(x.Address))
            .WithMessage("At least one field (Name, Email, PhoneNumber, or Address) must be provided for update.");

        // 2. Sahələr dolu olduğu halda yoxlanılan qaydalar (When istifadə edərək)

        RuleFor(x => x.Name)
            .MinimumLength(2).WithMessage("Name must be at least 2 characters long.")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.")
            .Matches(@"^[a-zA-Z\s\-']+$").WithMessage("Name contains invalid characters.")
            .When(x => !string.IsNullOrEmpty(x.Name)); // Yalnız boş deyilsə yoxla

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(150).WithMessage("Email is too long.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid international phone number format.")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        RuleFor(x => x.Address)
            .MinimumLength(10).WithMessage("Address is too short.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.")
            .When(x => !string.IsNullOrEmpty(x.Address));
    }
}