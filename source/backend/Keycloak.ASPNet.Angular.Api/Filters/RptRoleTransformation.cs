using Keycloak.ASPNet.Angular.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Filters;

/// <summary>
/// Transforms Keycloak roles in the resource_access claim to JWT role claims.
/// </summary>
/// <example>
/// "authorization": {
///   "permissions": [{
///       "scopes": [ "Read", "Manage" ],
///       "rsid": "a7ff18e5-fc31-4a3f-8c0e-4e2ea2943f41",
///       "rsname": "Articles"
///     }
///   ]
/// }
/// </example>
/// <seealso cref="IClaimsTransformation" />
public class RptRoleTransformation : IClaimsTransformation
{
    /// <summary>
    /// Provides a central transformation point to change the specified principal.
    /// Note: this will be run on each AuthenticateAsync call, so its safer to
    /// return a new ClaimsPrincipal if your transformation is not idempotent.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> to transform.</param>
    /// <returns>
    /// The transformed principal.
    /// </returns>
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
            return Task.FromResult(result);

        var authorization = principal.FindFirst("authorization")?.Value;
        if (string.IsNullOrWhiteSpace(authorization))
            return Task.FromResult(result);

        var permissionContainer = JsonConvert.DeserializeObject<KeycloakJwtPermissionContainer>(authorization);
        if (permissionContainer == null)
            return Task.FromResult(result);

        var permissions = permissionContainer
            .Permissions
            .Where(HasResourceNameAndScopes)
            .ToList();

        var clientRoles = permissions
            .SelectMany(permission => permission.Scopes.Select(scope => $"{permission.ResourceName}#{scope}"))
            .Select(role => new Claim(ClaimsIdentity.DefaultRoleClaimType, role))
            .ToList();

        identity.AddClaims(clientRoles);

        return Task.FromResult(result);
    }

    private static bool HasResourceNameAndScopes(KeycloakJwtPermission permission)
        => !string.IsNullOrWhiteSpace(permission.ResourceName) &&
           permission.Scopes != null &&
           permission.Scopes.Any();
}