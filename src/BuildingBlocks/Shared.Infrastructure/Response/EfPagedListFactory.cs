using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Response;

namespace Shared.Infrastructure.Response;

public class EfPagedListFactory : IPagedListFactory
{
    public async Task<PagedList<T>> CreateAsync<T>(
        IQueryable<T> source,
        int pageNumber,
        int pageSize,
        CancellationToken ct = default)
    {
        var count = await source.CountAsync(ct);

        var items = await source
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedList<T>(
            items,
            count,
            pageNumber,
            pageSize);
    }
}