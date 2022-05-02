using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Abstractions.IoC;

public interface IEndpointDefintion
{
    public void DefineEndpoints(WebApplication app);

    public void DefineServices(IServiceCollection services);

}
