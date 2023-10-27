using Microsoft.AspNetCore.Authorization;

namespace Keycloak.ASPNet.Angular.Api.Policies;

/// <summary>
/// Policy that checks if the route matches the requested resource.
/// </summary>
public class UmaPolicy : IAuthorizationRequirement
{
    /// <summary>
    /// UMA policy used to protect resources with read access.
    /// </summary>
    public const string SCOPE_READ = "Uma#Read";

    /// <summary>
    /// UMA policy used to protect resources with write access.
    /// </summary>
    public const string SCOPE_MANAGE = "Uma#Manage";

    /// <summary>
    /// Scope required to access the resource.
    /// </summary>
    public string RequiredScope { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="UmaPolicy"/> class.
    /// </summary>
    /// <param name="requiredScope">The scope required to access the resource</param>
    public UmaPolicy(string requiredScope)
        => RequiredScope = requiredScope;
}