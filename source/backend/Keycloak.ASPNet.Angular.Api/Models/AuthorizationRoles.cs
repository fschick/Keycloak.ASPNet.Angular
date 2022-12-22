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
    public const string ArticlesRead = "Articles#Read";

    /// <summary>
    /// Can manage articles
    /// </summary>
    public const string ArticlesManage = "Articles#Manage";
}