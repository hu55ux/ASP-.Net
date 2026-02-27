using ASP_.NET_InvoiceManagementAuth.DTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagementAuth.Validators;

/// <summary>
/// Validator for the <see cref="CustomerQueryDTO"/> to ensure valid pagination, sorting, and filtering parameters.
/// </summary>
public class CustomerQueryValidator : AbstractValidator<CustomerQueryDTO>
{
    /// <summary>
    /// Initializes validation rules for query parameters including page bounds, allowed sort fields, and search term length.
    /// </summary>
    public CustomerQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        var validSortFields = new[] { "Name", "Email", "CreatedAt" };
        RuleFor(x => x.Sort)
            .Must(x => string.IsNullOrEmpty(x) || validSortFields.Contains(x))
            .WithMessage($"Sort field must be one of the following: {string.Join(", ", validSortFields)}.");

        var validDirections = new[] { "asc", "desc" };
        RuleFor(x => x.SortDirection)
            .Must(x => string.IsNullOrEmpty(x) || validDirections.Contains(x.ToLower()))
            .WithMessage("Sort direction must be either 'asc' or 'desc'.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(100).WithMessage("Search term is too long.")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));
    }
}