using Keycloak.ASPNet.Angular.Api.Extensions;
using Keycloak.ASPNet.Angular.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;

namespace Keycloak.ASPNet.Angular.Api.Startup;

/// <summary>
/// Startup code to register authentication related stuff.
/// </summary>
internal static class AuthenticationStartup
{
    /// <summary>
    /// Register authentication related services.
    /// </summary>
    /// <param name="services">The services to act on.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = jwtOptions.Authority;
                options.Audience = jwtOptions.Audience;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters { ValidateAudience = true, };
            });
    }

    /// <summary>
    /// Register authentication middleware.
    /// </summary>
    /// <param name="app">The app to act on.</param>
    public static void AddAuthentication(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }

    /// <summary>
    /// Adds generic authorization to OpenAPI UI.
    /// </summary>
    /// <param name="options">The options to act on.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddGenericAuthorization(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();

        var authority = jwtOptions.Authority?.Trim('/');
        if (authority == null)
            throw new InvalidOperationException("Configuration value 'JwtBearer:Authority' is missing");

        options.OperationFilter<AuthorizationOperationFilter>();
        options
            .AddSecurityDefinition(
                JwtBearerDefaults.AuthenticationScheme,
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OpenIdConnect,
                    OpenIdConnectUrl = new Uri($"{authority}/.well-known/openid-configuration")
                });

        options.AddSecurityRequirement();
    }

    /// <summary>
    /// Adds 'Authorization Code Flow' authorization to OpenAPI UI.
    /// </summary>
    /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or illegal values.</exception>
    /// <param name="options">The options to act on.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddAuthorizationCodeFlow(this SwaggerGenOptions options, IConfiguration configuration)
    {
        var jwtOptions = configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();
        var openIdConnectConfiguration = jwtOptions.GetOpenIdConnectConfiguration().GetAwaiter().GetResult();

        options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = new Uri(openIdConnectConfiguration.AuthorizationEndpoint),
                    TokenUrl = new Uri(openIdConnectConfiguration.TokenEndpoint),
                }
            },
        });

        options.AddSecurityRequirement();
    }

    /// <summary>
    /// Adds authentication to OpenAPI UI.
    /// </summary>
    /// <param name="options">The options to act on.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void AddAuthentication(this SwaggerUIOptions options, IConfiguration configuration)
    {
        var clientId = configuration.GetSection("Swagger").GetValue<string>("ClientId");
        options.OAuthClientId(clientId);
        options.OAuthUsePkce();
        options.EnablePersistAuthorization();
    }

    private static void AddSecurityRequirement(this SwaggerGenOptions options)
    {
        var securityRequirement = new OpenApiSecurityRequirement
        { {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme,
                }
            },
            new List<string>()
        } };

        options.AddSecurityRequirement(securityRequirement);
    }
}