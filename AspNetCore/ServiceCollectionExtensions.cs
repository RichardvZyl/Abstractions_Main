using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;

namespace Abstractions.AspNetCore;

public static class ServiceCollectionExtensions
{
    public static void AddControllersDefault(this IServiceCollection services)
    {
        static void MvcOptions(MvcOptions options)
            => options.Filters.Add(new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()));

        static void JsonOptions(JsonOptions options)
            => options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;

        _ = services.AddControllers(MvcOptions).AddJsonOptions(JsonOptions);
    }

    public static void AddFileExtensionContentTypeProvider(this IServiceCollection services)
        => services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();

    public static void ConfigureFormOptions(this IServiceCollection services)
        => services.Configure<FormOptions>(options =>
           {
               options.ValueLengthLimit = int.MaxValue;
               options.MultipartBodyLengthLimit = int.MaxValue;
           });
}
