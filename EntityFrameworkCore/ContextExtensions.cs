using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Abstractions.EntityFrameworkCore;

public static class ContextExtensions
{
    /// <summary> Define the current SQL context of the application </summary>
    public static void AddContextUseSQL<TContext>(this IServiceCollection services, string? connectionName = null) where TContext : DbContext
    {
        var connectionString = AspNetCore.ServiceExtensions.GetConnectionString(services, connectionName ?? $"{typeof(TContext).Name}");

        services.AddContextMigrate<TContext>(options => options.UseSqlServer(connectionString));
    }

    /// <summary> Define the current PostgreSQL context of the application  </summary>
    public static void AddContextUsePostgreSQL<TContext>(this IServiceCollection services, string? connectionName = null) where TContext : DbContext
    {
        var connectionString = AspNetCore.ServiceExtensions.GetConnectionString(services, connectionName ?? $"{typeof(TContext).Name}");

        services.AddContextMigrate<TContext>(options => options.UseNpgsql(connectionString));
    }

    /// <summary> Define the current InMemoryDatabase context of the application </summary>
    public static void AddContextUseInMemory<TContext>(this IServiceCollection services, string? connectionName = null) where TContext : DbContext
    {
        services.AddDbContextPool<TContext>(options
            => options.UseInMemoryDatabase(connectionName ?? $"{nameof(TContext)}"));

        _ = services.BuildServiceProvider().GetRequiredService<TContext>().Database.EnsureCreated();
    }

    public static void AddContextUseSql<TContext>(this WebApplicationBuilder builder, string? connectionName = null) where TContext : DbContext
    {
        var connectionString = builder.Configuration.GetConnectionString(connectionName ?? $"{typeof(TContext).Name}");

        _ = builder.Services.AddDbContext<TContext>(options =>
            options.UseSqlServer(connectionString));
    }

    /// <summary> Apply Migrations </summary>
    private static void AddContextMigrate<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder> options) where TContext : DbContext
    {
        _ = services.AddDbContextPool<TContext>(options);

        services.BuildServiceProvider().GetRequiredService<TContext>().Database.Migrate();
    }

    public static void AddIdentity<TUser, URole>(this WebApplicationBuilder builder) where TUser : IdentityUser where URole : IdentityRole
        => builder.Services.AddIdentity<TUser, URole>(options => options.SignIn.RequireConfirmedAccount = true);

    public static void AddIdentityStores<TUser, UContext>(this WebApplicationBuilder builder) where TUser : IdentityUser where UContext : DbContext
        => builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<UContext>();

    public static void AddIdentityServerCore<TContext>(this IServiceCollection serviceCollection) where TContext : DbContext
    {
        var builder = serviceCollection.AddIdentityCore<IdentityUser>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        });

        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole<Guid>), builder.Services);
        _ = builder.AddEntityFrameworkStores<TContext>().AddDefaultTokenProviders();
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
