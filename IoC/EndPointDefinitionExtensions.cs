using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public interface IEndpointDefintion
{
    public void DefineEndpoints(WebApplication app);

    public void DefineServices(IServiceCollection services);

}

public static class EndPointDefinitionExtensions
{
    public static void AddEndpointDefintions(this IServiceCollection services, params Type[] scanMarkers)
    {
        var endpointDefinitions = new List<IEndpointDefintion>();

        foreach (var marker in scanMarkers)
        {
            endpointDefinitions.AddRange(
                marker.Assembly.ExportedTypes
                        .Where(x => typeof(IEndpointDefintion).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                        .Select(Activator.CreateInstance).Cast<IEndpointDefintion>()
                    );
        }

        foreach (var endpointDefinition in endpointDefinitions)
            endpointDefinition.DefineServices(services);


        _ = services.AddSingleton(endpointDefinitions as IReadOnlyCollection<IEndpointDefintion>);
    }

    public static void UseEndpointDefintions(this WebApplication app)
    {
        var definitions = app.Services.GetRequiredService<IReadOnlyCollection<IEndpointDefintion>>();

        foreach (var definition in definitions)
            definition.DefineEndpoints(app);
    }
}
