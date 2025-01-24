namespace Keycloak.ASPNet.Angular.Api.Models;

/// <summary>
/// Keycloak configuration.
/// </summary>
public class KeycloakConfiguration
{
    /// <summary>
    /// Name of the configuration section.
    /// </summary>
    public const string CONFIGURATION_SECTION = "Keycloak";

    /// <summary>
    /// Keycloak resource server configuration.
    /// </summary>

    public ResourceServerConfiguration ResourceServer { get; set; }

    /// <summary>
    /// Keycloak resource server configuration.
    /// </summary>
    public class ResourceServerConfiguration
    {
        /// <summary>
        /// The client id of the resource server.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Client secret to access the resource server.
        /// </summary>
        public string ClientSecret { get; set; }
    }
}