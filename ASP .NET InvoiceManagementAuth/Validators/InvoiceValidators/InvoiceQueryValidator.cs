using ASP_.NET_InvoiceManagementAuth.DTOs.InvoiceDTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagementAuth.Validators;

/// <summary>
/// Validator for the <see cref="InvoiceQueryDTO"/> to ensure that pagination, sorting, and search parameters follow defined business constraints.
/// </summary>
public class InvoiceQueryValidator : AbstractValidator<InvoiceQueryDTO>
{
    /// <summary>
    /// Initializes validation rules for invoice querying, including page limits, whitelisted sort fields, and search term length.
    /// </summary>
    public InvoiceQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        var validSortFields = new[] { "StartDate", "EndDate", "TotalSum", "CreatedAt" };
        RuleFor(x => x.Sort)
            .Must(x => string.IsNullOrEmpty(x) || validSortFields.Contains(x))
            .WithMessage($"Sort field must be one of the following: {string.Join(", ", validSortFields)}.");

        var validDirections = new[] { "asc", "desc" };
        RuleFor(x => x.SortDirection)
            .Must(x => string.IsNullOrEmpty(x) || validDirections.Contains(x.ToLower()))
            .WithMessage("Sort direction must be either 'asc' or 'desc'.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(50).WithMessage("Search term is too long.")
            .When(x => !string.IsNullOrEmpty(x.SearchTerm));
    }
}