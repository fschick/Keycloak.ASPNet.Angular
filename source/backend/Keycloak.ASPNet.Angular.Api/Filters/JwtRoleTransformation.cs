﻿using Microsoft.AspNetCore.Authentication;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Filters;

/// <summary>
/// Transforms provider specific roles to default role claims.
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
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via DI.")]
public class JwtRoleTransformation : IClaimsTransformation
{
    private readonly JwtRoleTransformationTenant _tenantRoleTransformation;
    private readonly JwtRoleTransformationAudience _audienceRoleTransformation;
    private readonly JwtRoleTransformationResource _resourceRoleTransformation;

    /// <summary>
    /// Initializes a new instance of the <see cref="JwtRoleTransformation"/> class.
    /// </summary>
    /// <param name="tenantRoleTransformation">The tenant role transformation.</param>
    /// <param name="audienceRoleTransformation">The audience role transformation.</param>
    /// <param name="resourceRoleTransformation">The resource role transformation.</param>
    public JwtRoleTransformation(JwtRoleTransformationTenant tenantRoleTransformation, JwtRoleTransformationAudience audienceRoleTransformation, JwtRoleTransformationResource resourceRoleTransformation)
    {
        _tenantRoleTransformation = tenantRoleTransformation;
        _audienceRoleTransformation = audienceRoleTransformation;
        _resourceRoleTransformation = resourceRoleTransformation;
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
        result = await _tenantRoleTransformation.TransformAsync(result);
        result = await _audienceRoleTransformation.TransformAsync(result);
        result = await _resourceRoleTransformation.TransformAsync(result);
        return result;
    }
}