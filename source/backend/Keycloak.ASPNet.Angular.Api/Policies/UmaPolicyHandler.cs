using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Policies;

/// <summary>
/// Policy that checks if the route matches the requested resource.
/// </summary>
public class UmaPolicyHandler : AuthorizationHandler<UmaPolicy>
{
    private readonly HttpClient _httpClient;
    private readonly JwtBearerOptions _jwtBearerConfiguration;

    /// <summary>
    /// Initializes a new instance of the <see cref="UmaPolicy"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="configuration">Application configuration.</param>
    public UmaPolicyHandler(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _jwtBearerConfiguration = configuration.GetSection("JwtBearer").Get<JwtBearerOptions>();
    }

    /// <inheritdoc />
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, UmaPolicy requirement)
    {
        if (context.Resource is not HttpContext httpContext)
            return;

        var requestedResourceUri = httpContext.Request.Path.Value;
        var requestedResourceScope = httpContext.Request.Method;

        if (requestedResourceUri == null)
            return;


        var token = await httpContext.GetTokenAsync(JwtBearerDefaults.AuthenticationScheme, "access_token");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var tokenEndpoint = $"{_jwtBearerConfiguration.Authority}/protocol/openid-connect/token";
        var formData = GetResourceRequestFormData(requestedResourceUri, requestedResourceScope);
        var response = await _httpClient.PostAsync(tokenEndpoint, formData);
        if (response.IsSuccessStatusCode)
            context.Succeed(requirement);
    }

    private FormUrlEncodedContent GetResourceRequestFormData(string resourceUri, string scope)
    {
        // https://www.keycloak.org/docs/latest/authorization_services/index.html#_service_obtaining_permissions
        var formData = new Dictionary<string, string>
        {
            ["grant_type"] = "urn:ietf:params:oauth:grant-type:uma-ticket",
            ["audience"] = _jwtBearerConfiguration.Audience,
            ["permission"] = $"{resourceUri}#{scope}",
            ["permission_resource_format"] = "uri",
            ["permission_resource_matching_uri"] = "true",
            ["response_mode"] = "decision",
        };

        return new FormUrlEncodedContent(formData);
    }
}