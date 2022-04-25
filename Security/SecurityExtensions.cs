using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Abstractions.Security;

public static class SecurityExtensions
{
    public static void AddCryptography(this IServiceCollection services, string key)
        => services.AddSingleton<ICryptographyService>(_ => new CryptographyService(key));

    public static void AddHash(this IServiceCollection services, int iterations, int size)
        => services.AddSingleton<IHashService>(_ => new HashService(iterations, size));

    public static void AddJsonWebToken(this IServiceCollection services, string key, TimeSpan expires)
        => services.AddJsonWebToken(new JsonWebTokenSettings(key, expires));

    public static void AddJsonWebToken(this IServiceCollection services, string key, TimeSpan expires, string audience, string issuer)
        => services.AddJsonWebToken(new JsonWebTokenSettings(key, expires, audience, issuer));

    public static void AddJsonWebToken(this IServiceCollection services, JsonWebTokenSettings jsonWebTokenSettings)
    {
        _ = services.AddSingleton(_ => jsonWebTokenSettings);
        _ = services.AddSingleton<IJsonWebTokenService, JsonWebTokenService>();
    }

    public static void AddAuthenticationJwtBearer(this IServiceCollection services)
    {
        var jsonWebTokenSettings = services.BuildServiceProvider().GetRequiredService<JsonWebTokenSettings>();

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jsonWebTokenSettings.Key));

        void JwtBearer(JwtBearerOptions options) => options.TokenValidationParameters = new TokenValidationParameters
        {
            IssuerSigningKey = securityKey,
            ValidAudience = jsonWebTokenSettings.Audience,
            ValidIssuer = jsonWebTokenSettings.Issuer,
            ValidateAudience = !string.IsNullOrEmpty(jsonWebTokenSettings.Audience),
            ValidateIssuer = !string.IsNullOrEmpty(jsonWebTokenSettings.Issuer),
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearer);
    }

    /// <summary> Add security token </summary>
    public static void AddSecurity(this IServiceCollection services)
    {
        services.AddHash(10000, 128);
        services.AddJsonWebToken(Guid.NewGuid().ToString(), TimeSpan.FromHours(12));
        services.AddAuthenticationJwtBearer();
    }
}
