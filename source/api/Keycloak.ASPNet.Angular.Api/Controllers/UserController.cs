using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Keycloak.ASPNet.Angular.Api.DTOs;
using Keycloak.ASPNet.Angular.Api.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Keycloak.ASPNet.Angular.Api.Controllers;

/// <summary>
/// User information API.
/// </summary>
[ApiV1Controller]
public class UserController : ControllerBase
{
    /// <summary>
    /// Gets the product name
    /// </summary>
    /// <param name="cancellationToken"> a token that allows processing to be cancelled.</param>
    /// <returns>
    /// The product name
    /// </returns>
    [HttpGet]
    [Authorize]
    public Task<UserDto> GetCurrentUser(CancellationToken cancellationToken = default)
    {
        var user = HttpContext.User;
        var userFound = user.Identity != null;
        if (!userFound)
            return null;

        var userInfo = new UserDto
        {
            AuthenticationType = user.Identity.AuthenticationType,
            IsAuthenticated = user.Identity.IsAuthenticated,
            Username = user.Identity.Name,
            Claims = user.Claims.Select(claim => new ClaimDto { Type = claim.Type, Value = claim.Value })
        };

        return Task.FromResult(userInfo);
    }
}