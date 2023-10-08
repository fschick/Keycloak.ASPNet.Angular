```csharp
services.AddSingleton<RptRoleTransformation, RptRoleTransformation>();

public class KeycloakJwtTransformation : IClaimsTransformation
{
    private readonly RealmRoleTransformation _realmRoleTransformation;
    private readonly ClientRoleTransformation _clientRoleTransformation;
    private readonly RptRoleTransformation _rptRoleTransformation;

    public KeycloakJwtTransformation(RealmRoleTransformation realmRoleTransformation, ClientRoleTransformation clientRoleTransformation, RptRoleTransformation rptRoleTransformation)
    {
        _realmRoleTransformation = realmRoleTransformation;
        _clientRoleTransformation = clientRoleTransformation;
        _rptRoleTransformation = rptRoleTransformation;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        result = await _realmRoleTransformation.TransformAsync(result);
        result = await _clientRoleTransformation.TransformAsync(result);
        result = await _rptRoleTransformation.TransformAsync(result);
        return result;
    }
}

public class RptRoleTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
            return Task.FromResult(result);

        var authorization = principal.FindFirst("authorization")?.Value;
        if (string.IsNullOrWhiteSpace(authorization))
            return Task.FromResult(result);

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var permissionContainer = JsonSerializer.Deserialize<KeycloakJwtPermissionContainer>(authorization, jsonSerializerOptions);
        if (permissionContainer == null)
            return Task.FromResult(result);

        var clientRoles = (permissionContainer.Permissions ?? new())
            .Where(HasResourceNameAndScopes)
            .SelectMany(permission => (permission.Scopes ?? new()).Select(scope => $"{permission.ResourceName}#{scope}"))
            .ToList();

        foreach (var role in clientRoles)
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return Task.FromResult(result);
    }

    private static bool HasResourceNameAndScopes(KeycloakJwtPermission permission)
        => !string.IsNullOrWhiteSpace(permission.ResourceName) &&
           permission.Scopes != null &&
           permission.Scopes.Any();
}

public class KeycloakJwtPermissionContainer
{
    public List<KeycloakJwtPermission>? Permissions { get; set; }
}

public class KeycloakJwtPermission
{
    [JsonPropertyName("rsid")]
    public string? Id { get; set; }

    [JsonPropertyName("rsname")]
    public string? ResourceName { get; set; }

    public List<string>? Scopes { get; set; }
}
```