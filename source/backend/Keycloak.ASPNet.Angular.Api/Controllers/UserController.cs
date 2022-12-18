using JWT;
using JWT.Serializers;
using Keycloak.ASPNet.Angular.Api.DTOs;
using Keycloak.ASPNet.Angular.Api.Routing;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Controllers;

/// <summary>
/// User information API.
/// </summary>
[ApiV1Controller]
public class UserController : ControllerBase
{
    /// <summary>
    /// Gets information about current user
    /// </summary>
    /// <param name="cancellationToken"> a token that allows processing to be cancelled.</param>
    /// <returns>
    /// Information about the current user.
    /// </returns>
    [HttpGet(nameof(GetCurrentUser))]
    [Authorize]
    public async Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var user = HttpContext.User;
        var userFound = user.Identity != null;
        if (!userFound)
            return null;

        var accessToken = await HttpContext.GetTokenAsync("access_token");
        var jwtPayload = DecodePayload(accessToken);

        var userInfo = new UserDto
        {
            AuthenticationType = user.Identity.AuthenticationType,
            IsAuthenticated = user.Identity.IsAuthenticated,
            Username = user.Identity.Name,
            Claims = user.Claims.Select(claim => new ClaimDto { Type = claim.Type, Value = claim.Value }),
            Payload = jwtPayload,
            AccessToken = accessToken,
        };

        return userInfo;
    }

    private static Dictionary<string, object> DecodePayload(string accessToken)
    {
        var serializer = new JsonNetSerializer();
        var urlEncoder = new JwtBase64UrlEncoder();
        var decoder = new JwtDecoder(serializer, urlEncoder);
        var obj = decoder.DecodeToObject(accessToken, false);
        return (Dictionary<string, object>)obj;
    }
}