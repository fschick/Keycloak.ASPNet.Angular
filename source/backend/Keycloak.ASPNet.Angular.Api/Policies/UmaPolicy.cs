using Microsoft.AspNetCore.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.Policies;

/// <summary>
/// Policy that checks if the route matches the requested resource.
/// </summary>
[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via DI.")]
public class UmaPolicy : IAuthorizationRequirement
{
    /// <summary>
    /// The name of the policy.
    /// </summary>
    public const string NAME = "UmaPolicy";
}