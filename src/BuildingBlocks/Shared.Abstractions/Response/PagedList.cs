namespace Shared.Abstractions.Response;

public class PagedList<T>
{
    public IReadOnlyList<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public PagedList(IReadOnlyList<T> items, int totalCount, int pageNumber, int pageSize)
    {
        Items = items;
        TotalCount = totalCount;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}
public interface IPagedListFactory
{
    Task<PagedList<T>> CreateAsync<T>(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default);
}