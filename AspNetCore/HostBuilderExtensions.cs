﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Abstractions.AspNetCore;

public static class HostBuilderExtensions
{
    public static void Run<T>(this IHostBuilder host) where T : class
        => host.ConfigureWebHostDefaults(builder => builder.UseStartup<T>()).Build().Run();
}
