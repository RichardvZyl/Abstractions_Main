using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Abstractions.IoC;

namespace Abstractions.EntityFrameworkCore;

public static class Extensions
{
    /// <summary> Define the current SQL context of the application </summary>
    public static void AddContextUseSQL<T>(this IServiceCollection services) where T : DbContext
    {
        var connectionString = IoCExtensions.GetConnectionString(services, $"{typeof(T).Name}_SQL");

        services.AddContextMigrate<T>(options => options.UseSqlServer(connectionString));
    }

    /// <summary> Define the current PostgreSQL context of the application  </summary>
    public static void AddContextUsePostgreSQL<T>(this IServiceCollection services) where T : DbContext
    {
        var connectionString = IoCExtensions.GetConnectionString(services, $"{typeof(T).Name}_Postgres");

        services.AddContextMigrate<T>(options => options.UseNpgsql(connectionString));
    }

    /// <summary> Define the current InMemoryDatabase context of the application </summary>
    public static void AddContextUseMemory<T>(this IServiceCollection services) where T : DbContext
    {
        services.AddDbContextPool<T>(options
            => options.UseInMemoryDatabase($"{typeof(T).Name}_InMemory"));

        _ = services.BuildServiceProvider().GetRequiredService<T>().Database.EnsureCreated();
    }

    /// <summary> Apply Migrations </summary>
    public static void AddContextMigrate<T>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where T : DbContext
    {
        _ = services.AddDbContextPool<T>(options);

        services.BuildServiceProvider().GetRequiredService<T>().Database.Migrate();
    }

    /// <summary> Apply Migrations </summary>
    public static DbSet<T> CommandSet<T>(this DbContext context) where T : class
        => context.DetectChangesLazyLoading(true).Set<T>();

    /// <summary> Keeps track of changes to the current unit of work </summary>
    public static DbContext DetectChangesLazyLoading(this DbContext context, bool enabled)
    {
        context.ChangeTracker.AutoDetectChangesEnabled = enabled;
        context.ChangeTracker.LazyLoadingEnabled = enabled;
        context.ChangeTracker.QueryTrackingBehavior = enabled
            ? QueryTrackingBehavior.TrackAll
            : QueryTrackingBehavior.NoTracking;

        return context;
    }

    /// <summary> Applies no tracking to the active result set for Querying purposes </summary>
    public static IQueryable<T> QuerySet<T>(this DbContext context) where T : class
        => context.DetectChangesLazyLoading(false).Set<T>().AsNoTracking();
}
