using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.Models;

/// <summary>
/// Authorization role names
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class AuthorizationRoles
{
    /// <summary>
    /// Read resources.
    /// </summary>
    public const string Read = nameof(Read);

    /// <summary>
    /// Manage resources.
    /// </summary>
    public const string Manage = nameof(Manage);
}