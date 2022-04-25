using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Abstractions.Repositories;

public static class SpecificationExtensions
{
    public static IQueryable<T> Specify<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
        => queryable
            .Include(specification)
            .Where(specification)
            .OrderBy(specification)
            .OrderByDescending(specification)
            .SkipTake(specification);

    private static IQueryable<T> Include<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
        => specification.Includes.Aggregate(queryable, (current, include) => current.Include(include));

    private static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
        => specification.OrderBy == null || specification.OrderByDescending != null ? queryable : queryable.OrderBy(specification.OrderBy);

    private static IQueryable<T> OrderByDescending<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
        => specification.OrderByDescending == null || specification.OrderBy != null ? queryable : queryable.OrderByDescending(specification.OrderByDescending);

    private static IQueryable<T> SkipTake<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
        => specification.Take == 0 ? queryable : queryable.Skip(specification.Skip).Take(specification.Take);

    private static IQueryable<T> Where<T>(this IQueryable<T> queryable, ISpecification<T> specification) where T : class
        => specification.Where == null ? queryable : queryable.Where(specification.Where);
}
