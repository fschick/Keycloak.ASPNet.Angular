using Keycloak.ASPNet.Angular.Api.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

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
        services.AddSingleton<IClaimsTransformation, KeycloakJwtTransformation>();
    }

    /// <summary>
    /// Adds authorization to OpenAPI UI.
    /// </summary>
    /// <param name="options">The options to act on.</param>
    public static void AddAuthorization(this SwaggerUIOptions options)
    {
        options.OAuthClientId("api");
        options.OAuthUsePkce();
        options.EnablePersistAuthorization();
    }
}