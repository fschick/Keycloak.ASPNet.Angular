using Keycloak.ASPNet.Angular.Api.Models;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Filters;

/// <summary>
/// Transforms provider specific roles from tenant to default role claims.
/// </summary>
/// <example>
/// "realm_access": {
///   "roles": [
///   "manage",
///   "offline_access",
///   "uma_authorization"
///   ]
/// },
/// </example>
/// <seealso cref="IClaimsTransformation" />
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via DI.")]
public class JwtRoleTransformationTenant : IClaimsTransformation
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

        var realmAccess = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess))
            return Task.FromResult(result);

        var roleContainer = JsonConvert.DeserializeObject<KeycloakJwtRoleContainer>(realmAccess);
        if (roleContainer == null)
            return Task.FromResult(result);

        var realmRoles = roleContainer.Roles
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .ToList();

        foreach (var role in realmRoles)
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return Task.FromResult(result);
    }
}