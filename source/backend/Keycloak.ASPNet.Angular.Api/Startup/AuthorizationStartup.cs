using Keycloak.ASPNet.Angular.Api.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;

namespace Keycloak.ASPNet.Angular.Api.Startup;

/// <summary>
/// Startup code to register authorization related stuff.
/// </summary>
internal static class AuthorizationStartup
{
    /// <summary>
    /// Register authorization related services.
    /// </summary>
    /// <param name="services">The services to act on.</param>
    public static void AddAuthorization(this IServiceCollection services)
    {
        services.AddSingleton<RealmRoleTransformation, RealmRoleTransformation>();
        services.AddSingleton<ClientRoleTransformation, ClientRoleTransformation>();
        services.AddSingleton<RptRoleTransformation, RptRoleTransformation>();
        services.AddSingleton<IClaimsTransformation, KeycloakJwtTransformation>();
    }

    /// <summary>
    /// Adds authorization to OpenAPI UI.
    /// </summary>
    /// <param name="options">The options to act on.</param>
    /// <param name="app">The app to act on.</param>
    public static void AddAuthorization(this SwaggerUIOptions options, IApplicationBuilder app)
    {
        options.OAuthClientId("frontend");
        options.OAuthUsePkce();
        options.EnablePersistAuthorization();
        options.AddRptInterceptor(app);
    }

    private static void AddRptInterceptor(this SwaggerUIOptions options, IApplicationBuilder app)
    {
        var configuration = app.ApplicationServices.GetRequiredService<IConfiguration>();
        var jwtOptions = configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();
        var authority = jwtOptions.Authority?.Trim('/');
        if (authority == null)
            throw new ArgumentException("Configuration value missing", "JwtBearer.Authority");

        app.AddStaticFiles("Extensions");
        options.InjectJavascript("/Extensions/RptInterceptor.js");

        var tokenUrl = $"{authority}/protocol/openid-connect/token";
        var resourceServerId = jwtOptions.Audience;
        var rptInterceptFunction = $"(req) {{ return entitlement(req, '{tokenUrl}', '{resourceServerId}'); }}";
        options.UseRequestInterceptor(rptInterceptFunction);
    }

    private static void AddStaticFiles(this IApplicationBuilder app, string path)
    {
        var environment = app.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(Path.Combine(environment.ContentRootPath, path)),
            RequestPath = $"/{path}"
        });
    }
}