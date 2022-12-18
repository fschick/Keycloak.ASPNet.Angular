using System.Collections.Generic;

namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// Information about an user.
/// </summary>
public class UserDto
{
    /// <summary>
    /// The type of authentication.
    /// </summary>
    public string AuthenticationType { get; init; }

    /// <summary>
    /// Indicates whether the user is authenticated.
    /// </summary>
    public bool IsAuthenticated { get; init; }

    /// <summary>
    /// The user name.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    /// Claims corresponding to the user.
    /// </summary>
    public IEnumerable<ClaimDto> Claims { get; init; }

    /// <summary>
    /// Parsed access token used for current request.
    /// </summary>
    public Dictionary<string, object> Payload { get; init; }

    /// <summary>
    /// Raw access token used for current request.
    /// </summary>
    public string AccessToken { get; init; }
}