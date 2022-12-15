using Keycloak.ASPNet.Angular.Api.Extensions;
using Keycloak.ASPNet.Angular.Api.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace Keycloak.ASPNet.Angular.Api.Startup;

/// <summary>
/// Startup code to register generation of OpenAPI spec and UI.
/// </summary>
internal static class OpenApiStartup
{
    /// <summary>
    /// Register OpenAPI related services.
    /// </summary>
    /// <param name="services">The services to act on.</param>
    public static void AddOpenApi(this IServiceCollection services)
    {
        services.AddSwaggerGen(config =>
        {
            config.EnableAnnotations();

            const string documentName = ApiV1Controller.API_VERSION;
            var productName = AssemblyExtensions.GetProgramProduct();
            config.SwaggerDoc(documentName, new OpenApiInfo { Title = $"{productName} API", Version = ApiV1Controller.API_VERSION });

            var restXmlDoc = Path.Combine(AppContext.BaseDirectory, "Keycloak.ASPNet.Angular.Api.xml");
            config.IncludeXmlComments(restXmlDoc);
        });
    }

    /// <summary>
    /// Add OpenAPI spec generation middleware and UI.
    /// </summary>
    /// <param name="app">The app to act on.</param>
    public static void AddOpenApi(this WebApplication app)
    {
        app.AddSwaggerMiddleware();
        app.AddSwaggerUi();
        app.AddOpenApiUiRedirects();
    }

    private static void AddSwaggerMiddleware(this IApplicationBuilder app)
        => app.UseSwagger(c => c.RouteTemplate = $"{StaticRoutes.API_PREFIX}/{{documentName}}/{StaticRoutes.OPEN_API_SPEC}");

    private static void AddSwaggerUi(this IApplicationBuilder app)
    {
        app.UseSwaggerUI(config =>
        {
            config.RoutePrefix = StaticRoutes.API_PREFIX;
            config.SwaggerEndpoint($"{ApiV1Controller.API_VERSION}/{StaticRoutes.OPEN_API_SPEC}", $"API version {ApiV1Controller.API_VERSION}");
            config.DisplayRequestDuration();
            config.EnableDeepLinking();
            config.EnableTryItOutByDefault();
            config.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
        });
    }

    private static void AddOpenApiUiRedirects(this IEndpointRouteBuilder app)
    {
        app.MapGet($"/", redirectToOpenApiUi);
        app.MapGet($"/{StaticRoutes.OPEN_API_UI_ROUTE}{{**path}}", redirectToOpenApiUi);
        app.MapGet($"/{StaticRoutes.SWAGGER_UI_ROUTE}{{**path}}", redirectToOpenApiUi);

        static IResult redirectToOpenApiUi(HttpContext httpContext, string path)
        {
            var query = httpContext.Request.QueryString;
            var redirectUrl = $"/{StaticRoutes.API_PREFIX}/{path}{query}";
            return Results.Redirect(redirectUrl, true);
        }
    }
}