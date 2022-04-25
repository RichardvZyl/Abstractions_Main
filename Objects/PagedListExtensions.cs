namespace Abstractions.Objects;

public static class PagedListExtensions
{
    public static PagedList<T> List<T>(this IQueryable<T> queryable, PagedListParameters parameters)
        => new(queryable, parameters);

    public static Task<PagedList<T>> ListAsync<T>(this IQueryable<T> queryable, PagedListParameters parameters)
        => Task.FromResult(new PagedList<T>(queryable, parameters));
}
