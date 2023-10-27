using Microsoft.AspNetCore.Authorization;

namespace Keycloak.ASPNet.Angular.Api.Policies;

/// <summary>
/// Policy that checks if the route matches the requested resource.
/// </summary>
public class UmaPolicy : IAuthorizationRequirement
{
    /// <summary>
    /// The name of the policy.
    /// </summary>
    public const string NAME = "UmaPolicy";
}