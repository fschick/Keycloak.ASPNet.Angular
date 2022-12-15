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
    /// Gets the product name
    /// </summary>
    /// <param name="cancellationToken"> a token that allows processing to be cancelled.</param>
    /// <returns>
    /// The product name
    /// </returns>
    [HttpGet]
    public Task<object> GetCurrentUser(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}