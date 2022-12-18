using Keycloak.ASPNet.Angular.Api.Extensions;
using Keycloak.ASPNet.Angular.Api.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Controllers;

/// <summary>
/// Product information API.
/// </summary>
[ApiV1Controller]
public class InformationController : ControllerBase
{
    /// <summary>
    /// Gets the product name
    /// </summary>
    /// <param name="cancellationToken"> a token that allows processing to be cancelled</param>
    /// <returns>
    /// The product name
    /// </returns>
    [HttpGet(nameof(GetProductName))]
    [AllowAnonymous]
    public Task<string> GetProductName(CancellationToken cancellationToken = default)
        => Task.FromResult(AssemblyExtensions.GetProgramProduct());

    /// <summary>
    /// Gets the product version
    /// </summary>
    /// <param name="cancellationToken">a token that allows processing to be cancelled</param>
    /// <returns>
    /// The product version
    /// </returns>
    [HttpGet(nameof(GetProductVersion))]
    [AllowAnonymous]
    public Task<string> GetProductVersion(CancellationToken cancellationToken = default)
        => Task.FromResult(AssemblyExtensions.GetProgramProductVersion());
}