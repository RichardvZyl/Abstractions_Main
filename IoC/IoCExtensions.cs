using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Abstractions.IoC;

public static class IoCExtensions
{
    public static void AddClassesInterfaces(this IServiceCollection services, params Assembly[] assemblies)
        => services.Scan(scan => scan
             .FromAssemblies(assemblies)
             .AddClasses()
             .UsingRegistrationStrategy(RegistrationStrategy.Skip)
             .AsMatchingInterface()
             .WithScopedLifetime());

    public static void AddConfigurationsFromAssembly(this ModelBuilder modelBuilder)
    {
        static bool Expression(Type type)
            => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>);

        var types = Assembly.GetCallingAssembly().GetTypes().Where(type
            => type.GetInterfaces().Any(Expression)).ToList();

        var configurations = types.Select(Activator.CreateInstance).ToList();

        configurations.ForEach(configuration
            =>
            {
                if (configuration is null) return;
                modelBuilder.ApplyConfiguration((dynamic)configuration);
            });
    }
}
