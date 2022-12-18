using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// Information about an user.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Public DTO")]

public class UserDto
{
    /// <summary>
    /// The type of authentication.
    /// </summary>
    public string AuthenticationType { get; set; }

    /// <summary>
    /// Indicates whether the user is authenticated.
    /// </summary>
    public bool IsAuthenticated { get; set; }

    /// <summary>
    /// The username.
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Claims corresponding to the user.
    /// </summary>
    public IEnumerable<ClaimDto> Claims { get; set; }

    /// <summary>
    /// Parsed access token used for current request.
    /// </summary>
    public Dictionary<string, object> Payload { get; set; }

    /// <summary>
    /// Raw access token used for current request.
    /// </summary>
    public string AccessToken { get; set; }
}