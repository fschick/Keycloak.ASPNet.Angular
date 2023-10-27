using Keycloak.ASPNet.Angular.Api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Policies;

/// <summary>
/// Policy that checks if the route matches the requested resource.
/// </summary>
public class UmaPolicyHandler : AuthorizationHandler<UmaPolicy>
{
    private readonly HttpClient _httpClient;
    private readonly KeycloakConfiguration _keycloakConfiguration;
    private readonly JwtBearerOptions _jwtBearerConfiguration;
    private List<ResourceDescription> _resourceDescriptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="UmaPolicy"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client.</param>
    /// <param name="configuration">Application configuration.</param>
    public UmaPolicyHandler(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _keycloakConfiguration = configuration.GetSection(KeycloakConfiguration.CONFIGURATION_SECTION).Get<KeycloakConfiguration>();
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

        _resourceDescriptions ??= await LoadResourceDescriptions();

        var requestedResource = _resourceDescriptions
            .FirstOrDefault(resourceDescription => Regex.IsMatch(requestedResourceUri, WildcardToRegex(resourceDescription.Uri)));

        if (requestedResource == null)
            throw new InvalidOperationException($"No resource found for requested resource URI '{requestedResourceUri}'");

        var requiredRole = $"{requestedResource.Name}#{requestedResourceScope}";

        if (context.User.IsInRole(requiredRole))
            context.Succeed(requirement);
    }

    private async Task<List<ResourceDescription>> LoadResourceDescriptions()
    {
        var resourceEndpoint = await GetResourceEndpoint();
        var accessToken = await GetResourceServerAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var resourceIds = await _httpClient.GetFromJsonAsync<List<string>>(resourceEndpoint);
        var keycloakResourceDescriptions = await GetKeycloakResourceResourceDescriptions(resourceEndpoint, resourceIds);
        EnsureUniqueResourceNames(keycloakResourceDescriptions);

        var resourceDescriptions = keycloakResourceDescriptions
            .SelectMany(toResourceDescriptions)
            .OrderByDescending(pathPartCount)
            .ToList();

        return resourceDescriptions;

        static IEnumerable<ResourceDescription> toResourceDescriptions(KeycloakResourceDescription resourceDescription)
            => resourceDescription
                .Uris
                .Select(uri => new ResourceDescription
                {
                    Uri = uri,
                    Name = resourceDescription.Name,
                });

        static int pathPartCount(ResourceDescription resourceDescription)
            => resourceDescription.Uri.Split('/').Length;
    }

    private async Task<string> GetResourceEndpoint()
    {
        var dict = await _httpClient.GetFromJsonAsync<Dictionary<string, object>>($"{_jwtBearerConfiguration.Authority}/.well-known/uma2-configuration");
        return dict["resource_registration_endpoint"].ToString();
    }

    private async Task<string> GetResourceServerAccessToken()
    {
        var clientSecrets = new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", _keycloakConfiguration.ResourceServer.ClientId },
            { "client_secret", _keycloakConfiguration.ResourceServer.ClientSecret },
        };

        var authTokenUrl = $"{_jwtBearerConfiguration.Authority}/protocol/openid-connect/token";
        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, authTokenUrl) { Content = new FormUrlEncodedContent(clientSecrets) };
        var responseMessage = await _httpClient.SendAsync(tokenRequest);
        var responseContent = await responseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent);
        return response["access_token"];
    }

    private async Task<List<KeycloakResourceDescription>> GetKeycloakResourceResourceDescriptions(string resourceEndpoint, List<string> resourceIds)
    {
        var resourceDescriptions = new List<KeycloakResourceDescription>();
        foreach (var resourceId in resourceIds)
        {
            var resourceDescription = await _httpClient
                .GetFromJsonAsync<KeycloakResourceDescription>($"{resourceEndpoint}/{resourceId}");
            resourceDescriptions.Add(resourceDescription);
        }

        return resourceDescriptions;
    }

    private static void EnsureUniqueResourceNames(IEnumerable<KeycloakResourceDescription> resourceDescriptions)
    {
        var duplicateResourceNames = resourceDescriptions
            .GroupBy(resourceDescription => resourceDescription.Name)
            .Where(group => group.Count() > 1)
            .Select(group => $"{group.Key} ({string.Join(", ", group.Select(x => x.Id))})")
            .ToList();

        if (duplicateResourceNames.Any())
            throw new InvalidOperationException($"Duplicate resource names found: {string.Join(", ", duplicateResourceNames)}");
    }

    private static string WildcardToRegex(string pattern)
        => $"^{Regex.Escape(pattern)}$"
            .Replace("\\*", ".*")
            .Replace("\\?", ".");

    private class ResourceDescription
    {
        public string Uri { get; init; }
        public string Name { get; init; }
    }

    private class KeycloakResourceDescription
    {
        [JsonProperty("_id")]
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        public string Name { get; set; }

        public string[] Uris { get; set; }
    }
}