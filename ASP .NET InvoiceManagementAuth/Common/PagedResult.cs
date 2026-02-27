namespace ASP_.NET_InvoiceManagementAuth.Common;
/// <summary>
/// Get the result of the query in a paged format, which includes 
/// the total number of records and the data for the current page.
/// </summary>
public class PagedResult<T>
{
    /// <summary>
    /// Gets or sets the collection of items contained in the current instance.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();
    /// <summary>
    /// Page number of the current page.
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Page size of the current page.
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// Count of the total records in the database that match the query criteria, regardless of pagination.
    /// </summary>
    public int TotalCount { get; set; }
    /// <summary>
    /// Total number of pages available based on the total count and page size. Calculated as the 
    /// ceiling of the division of TotalCount by PageSize.
    /// </summary>
    public int TotalPages
        => Convert.ToInt32(Math.Ceiling(TotalCount / (double)(PageSize)));
    /// <summary>
    /// HasPrevious is a boolean property that indicates whether there are 
    /// previous pages available based on the current page number.
    /// </summary>
    public bool HasPrevious
        => Page > 1;
    /// <summary>
    /// HasNext is a boolean property that indicates whether there are 
    /// next pages available based on the current page number and total pages.
    /// </summary>
    public bool HasNext
        => Page < TotalPages;

    /// <summary>
    /// Creates a new instance of PagedResult&lt;T&gt; with the specified items, page number, page size, and total count.
    /// </summary>
    /// <param name="items">The collection of items for the current page.</param>
    /// <param name="page">The current page number.</param>
    /// <param name="pageSize">The size of the page.</param>
    /// <param name="totalCount">The total number of items matching the query.</param>
    /// <returns>A new instance of <see cref="PagedResult{T}"/>.</returns>
    public static PagedResult<T> Create(
                                    IEnumerable<T> items,
                                    int page,
                                    int pageSize,
                                    int totalCount)
    {
        return new PagedResult<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
}
