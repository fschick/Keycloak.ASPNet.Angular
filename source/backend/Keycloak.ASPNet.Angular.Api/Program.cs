using Keycloak.ASPNet.Angular.Api.Startup;
using Microsoft.AspNetCore.Builder;

namespace Keycloak.ASPNet.Angular.Api;

/// <summary>
/// Main entry-class for this application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry-point for this application.
    /// </summary>
    /// <param name="args">An array of command-line argument strings.</param>
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddRestApi();
        builder.Services.AddOpenApi(builder.Configuration);
        builder.Services.AddAuthentication(builder.Configuration);
        builder.Services.AddAuthorization(builder.Configuration);

        var app = builder.Build();

        app.AddRestApi();
        app.AddOpenApi();
        app.AddAuthentication();

        app.Run();
    }
}