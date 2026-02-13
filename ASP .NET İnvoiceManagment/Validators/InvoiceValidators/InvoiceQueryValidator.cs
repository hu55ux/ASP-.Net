using ASP_.NET_InvoiceManagment.DTOs.InvoiceDTOs;
using FluentValidation;

namespace ASP_.NET_InvoiceManagment.Validators;

/// <summary>
/// Validator class for validating the InvoiceQueryDTO. This class 
/// uses FluentValidation to define rules for pagination, sorting, and searching when querying invoices.
/// </summary>
public class InvoiceQueryValidator : AbstractValidator<InvoiceQueryDTO>
{


    /// <summary>
    /// Constructor for InvoiceQueryValidator. Here we define 
    /// all the validation rules for the InvoiceQueryDTO, including:
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