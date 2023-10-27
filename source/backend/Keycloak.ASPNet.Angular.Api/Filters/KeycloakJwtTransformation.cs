﻿using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Filters;

/// <summary>
/// Transforms Keycloak roles and UMA/RPT permissions to JWT role claims.
/// </summary>
/// <example>
/// {
///   "realm_access": {
///     "roles": [
///       "offline_access",
///       "uma_authorization"
///     ]
///   },
///   "resource_access": {
///     "api": {
///       "roles": [
///         "manage"
///       ]
///     },
///   },
///   "authorization": {
///     "permissions": [{
///         "scopes": [
///           "Read",
///           "Manage"
///         ],
///         "rsid": "a7ff18e5-fc31-4a3f-8c0e-4e2ea2943f41",
///         "rsname": "Articles"
///       }
///     ]
///   }
/// }
/// </example>
/// <seealso cref="IClaimsTransformation" />
public class KeycloakJwtTransformation : IClaimsTransformation
{
    private readonly RealmRoleTransformation _realmRoleTransformation;
    private readonly ClientRoleTransformation _clientRoleTransformation;
    private readonly RptRoleTransformation _rptRoleTransformation;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeycloakJwtTransformation"/> class.
    /// </summary>
    /// <param name="realmRoleTransformation">The realm role transformation.</param>
    /// <param name="clientRoleTransformation">The client role transformation.</param>
    /// <param name="rptRoleTransformation">The UMA/RTP role transformation.</param>
    /// <autogeneratedoc />
    public KeycloakJwtTransformation(RealmRoleTransformation realmRoleTransformation, ClientRoleTransformation clientRoleTransformation, RptRoleTransformation rptRoleTransformation)
    {
        _realmRoleTransformation = realmRoleTransformation;
        _clientRoleTransformation = clientRoleTransformation;
        _rptRoleTransformation = rptRoleTransformation;
    }

    /// <summary>
    /// Provides a central transformation point to change the specified principal.
    /// Note: this will be run on each AuthenticateAsync call, so its safer to
    /// return a new ClaimsPrincipal if your transformation is not idempotent.
    /// </summary>
    /// <param name="principal">The <see cref="T:System.Security.Claims.ClaimsPrincipal" /> to transform.</param>
    /// <returns>
    /// The transformed principal.
    /// </returns>
    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        result = await _realmRoleTransformation.TransformAsync(result);
        result = await _clientRoleTransformation.TransformAsync(result);
        result = await _rptRoleTransformation.TransformAsync(result);
        return result;
    }
}