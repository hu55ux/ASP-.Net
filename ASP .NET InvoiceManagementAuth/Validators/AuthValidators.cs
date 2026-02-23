using ASP_.NET_InvoiceManagementAuth.DTOs.Auth;
using FluentValidation;

namespace ASP_.NET_InvoiceManagementAuth.Validators;

/// <summary>
/// Validator for the login request to ensure required credentials are provided and properly formatted.
/// </summary>
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    /// <summary>
    /// Initializes validation rules for email format and password complexity during login.
    /// </summary>
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().WithMessage("Email is required")
           .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Passwords must be at least 6 characters.")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
                .WithMessage("Passwords must have at least one digit ('0'-'9'), " +
                "one lowercase ('a'-'z'), and one uppercase ('A'-'Z').");
    }
}

/// <summary>
/// Validator for the registration request to enforce data integrity for new user accounts.
/// </summary>
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    /// <summary>
    /// Initializes validation rules for names, email authenticity, password strength, and password confirmation.
    /// </summary>
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required")
            .MinimumLength(2).WithMessage("Firstname must be at least 2 characters long");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required")
            .MinimumLength(2).WithMessage("Lastname must be at least 2 characters long");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not valid");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Passwords must be at least 6 characters.")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)")
            .WithMessage("Passwords must have at least one digit ('0'-'9'), one lowercase ('a'-'z'), and one uppercase ('A'-'Z').");

        RuleFor(x => x.ConfirmedPassword)
            .NotEmpty().WithMessage("Confirmed is required")
            .Equal(x => x.Password).WithMessage("Passwords do not match");
    }
}