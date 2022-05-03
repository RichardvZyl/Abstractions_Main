using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Abstractions.SwaggerExtension;
public static class SwaggerBuilder
{
    public static void AddSwagger(this IApplicationBuilder application)
    {
        var appName = Assembly.GetEntryAssembly()?.GetName().Name;

        _ = application.UseSwagger();
        _ = application.UseSwaggerUI(c =>
          {
              c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"v1{appName}");
              // Specify an endpoint for v2
              c.SwaggerEndpoint($"/swagger/v2/swagger.json", $"v2{appName}");
          });
    }

    public static void AddSwagger(this WebApplication application)
    {
        var appName = Assembly.GetEntryAssembly()?.GetName().Name;

        _ = application.UseSwagger();
        _ = application.UseSwaggerUI(c
            => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"v1{appName}")
        );
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        var appName = Assembly.GetEntryAssembly()?.GetName().Name;

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{appName} v1", Version = "v1" });
            // Add a SwaggerDoc for v2 
            c.SwaggerDoc("v2",
                new OpenApiInfo
                {
                    Version = "v2",
                    Title = "DemoApplicationAPI v2"
                });
            c.OperationFilter<RemoveVersionParameterFilter>();
            c.DocumentFilter<ReplaceVersionWithExactValueInPathFilter>();
            c.EnableAnnotations();

            //c.DocInclusionPredicate((version, desc) =>
            //{
            //var versions = desc.ControllerAttributes()
            //    .OfType<ApiVersionAttribute>()
            //    .SelectMany(attr => attr.Versions);

            //var maps = desc.ActionAttributes()
            //    .OfType<MapToApiVersionAttribute>()
            //    .SelectMany(attr => attr.Versions)
            //    .ToArray();

            //    return versions.Any(v => $"v{v.ToString()}" == version)
            //                  && (!maps.Any() || maps.Any(v => $"v{v.ToString()}" == version)); ;
            //});

            var xmlFile = $"{appName}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            c.IncludeXmlComments(xmlPath);

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter into field the word 'Bearer' followed by a space and the the JWT token value",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                }
            });
        });
    }

    /// <summary> Add versioning functionality </summary>
    private class RemoveVersionParameterFilter : IOperationFilter
    {
        /// <summary> Add versioning functionality to Swagger </summary>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var versionParameter = operation.Parameters.Single(p => p.Name == "version");
            _ = operation.Parameters.Remove(versionParameter);
        }
    }

    /// <summary> Add current version </summary>
    private class ReplaceVersionWithExactValueInPathFilter : IDocumentFilter
    {
        /// <summary> Add current version </summary>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = new OpenApiPaths();
            foreach (var path in swaggerDoc.Paths)
            {
                paths.Add(path.Key.Replace("v{version}", swaggerDoc.Info.Version), path.Value);
            }
            swaggerDoc.Paths = paths;
        }
    }
}
