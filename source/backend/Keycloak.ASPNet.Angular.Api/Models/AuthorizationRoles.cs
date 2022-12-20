using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.Models;

/// <summary>
/// Authorization role names
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class AuthorizationRoles
{
    /// <summary>
    /// Manage role.
    /// </summary>
    public const string Manage = nameof(Manage);
}