using Keycloak.ASPNet.Angular.Api.Routing;
using Microsoft.AspNetCore.Mvc;
using System;
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
    public Task<object> GetCurrentUser(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}