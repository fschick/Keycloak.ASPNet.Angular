using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Keycloak.ASPNet.Angular.Api.Extensions;

/// <summary>
/// Extension methods for <see cref="JwtBearerOptions"/>
/// </summary>
public static class JwtBearerExtensions
{
    /// <summary>
    /// Retrieves the <see cref="OpenIdConnectConfiguration"/> from well known openid-configuration URL.
    /// </summary>
    /// <param name="jwtBearerOptions"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<OpenIdConnectConfiguration> GetOpenIdConnectConfiguration(this JwtBearerOptions jwtBearerOptions)
    {
        var httpClient = new HttpClient();
        if (jwtBearerOptions.Authority == null)
            throw new InvalidOperationException("Configuration JwtBearer.Authority not set.");

        var baseUri = new Uri(jwtBearerOptions.Authority.TrimEnd('/') + '/');
        var url = new Uri(baseUri, ".well-known/openid-configuration");

        var response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var configurationJson = await response.Content.ReadAsStringAsync();
        return new OpenIdConnectConfiguration(configurationJson);
    }
}