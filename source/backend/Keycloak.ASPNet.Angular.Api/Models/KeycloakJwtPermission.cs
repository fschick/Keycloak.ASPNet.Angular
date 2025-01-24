// ReSharper disable StringLiteralTypo
// 
using Newtonsoft.Json;

namespace Keycloak.ASPNet.Angular.Api.Models;

/// <summary>
/// UMA authorization permission.
/// </summary>
public class KeycloakJwtPermission
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [JsonProperty("rsid")]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the resource.
    /// </summary>
    [JsonProperty("rsname")]
    public string ResourceName { get; set; }

    /// <summary>
    /// Gets or sets the scopes of the resource.
    /// </summary>
    public string[] Scopes { get; set; }
}
