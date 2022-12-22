using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.Models;

/// <summary>
/// Authorization role names
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class AuthorizationRoles
{
    /// <summary>
    /// Can read articles
    /// </summary>
    public const string ArticlesRead = "Article#GET";

    /// <summary>
    /// Can manage articles
    /// </summary>
    public const string ArticlesCreate = "Article#POST";

    /// <summary>
    /// Can manage articles
    /// </summary>
    public const string ArticlesDelete = "Article#DELETE";
}