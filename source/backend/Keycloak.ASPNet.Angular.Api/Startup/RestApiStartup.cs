using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Keycloak.ASPNet.Angular.Api.Startup;

/// <summary>
/// Startup code to register REST API services and configuration (CORS, ...).
/// </summary>
internal static class RestApiStartup
{
    /// <summary>
    /// Register REST API related services.
    /// </summary>
    /// <param name="services">The services to act on.</param>
    public static void AddRestApi(this IServiceCollection services)
        => services
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                var camelCase = new CamelCaseNamingStrategy();
                options.SerializerSettings.Converters.Add(new StringEnumConverter(camelCase));
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

    /// <summary>
    /// Register REST routes and configure CORS.
    /// </summary>
    /// <param name="app">The app to act on.</param>
    public static void AddRestApi(this WebApplication app)
    {
        app.MapControllers();
        app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    }
}