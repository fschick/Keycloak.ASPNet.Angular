using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.Routing;

[AttributeUsage(AttributeTargets.Class)]
[ExcludeFromCodeCoverage]
internal class ApiV1Controller : ControllerAttribute, IApiBehaviorMetadata, IRouteTemplateProvider, IApiDescriptionGroupNameProvider
{
    public const string API_VERSION = "v1";

    /// <inheritdoc />
    public string GroupName => API_VERSION;

    /// <inheritdoc />
    public string Template => $"{StaticRoutes.API_PREFIX}/{API_VERSION}/[controller]";

    /// <inheritdoc />
    public int? Order => 0;

    /// <inheritdoc />
    public string Name => "[controller]_[action]";
}