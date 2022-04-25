using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Abstractions.AspNetCore;

public static class ApplicationBuilderExtensions
{
    public static void UseCorsAllowAny(this IApplicationBuilder application) 
        => application.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

    public static void UseEndpoints(this IApplicationBuilder application) 
        => application.UseEndpoints(builder => builder.MapControllers());

    public static void UseException(this IApplicationBuilder application)
    {
        var environment = application.ApplicationServices.GetRequiredService<IWebHostEnvironment>();

        if (environment.IsDevelopment())
            _ = application.UseDeveloperExceptionPage();
    }

    public static void UseHttps(this IApplicationBuilder application)
    {
        _ = application.UseHsts();
        _ = application.UseHttpsRedirection();
    }
}
