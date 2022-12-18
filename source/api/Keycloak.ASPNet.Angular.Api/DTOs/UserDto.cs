using System.Collections.Generic;

namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// Information about an user.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or initializes the type of the authentication.
    /// </summary>
    public string AuthenticationType { get; init; }

    /// <summary>
    /// Gets or initializes a value indicating whether the user is authenticated.
    /// </summary>
    public bool IsAuthenticated { get; init; }

    /// <summary>
    /// Gets or initializes the user name.
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    /// Access to Authentication 'type' info.
    /// </summary>
    public IEnumerable<ClaimDto> Claims { get; init; }
}