```csharp
builder.Services.AddSingleton<RealmRoleTransformation, RealmRoleTransformation>();
builder.Services.AddSingleton<ClientRoleTransformation, ClientRoleTransformation>();
builder.Services.AddSingleton<IClaimsTransformation, KeycloakJwtTransformation>();

public class KeycloakJwtTransformation : IClaimsTransformation
{
    private readonly RealmRoleTransformation _realmRoleTransformation;
    private readonly ClientRoleTransformation _clientRoleTransformation;

    public KeycloakJwtTransformation(RealmRoleTransformation realmRoleTransformation, ClientRoleTransformation clientRoleTransformation)
    {
        _realmRoleTransformation = realmRoleTransformation;
        _clientRoleTransformation = clientRoleTransformation;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        result = await _realmRoleTransformation.TransformAsync(result);
        result = await _clientRoleTransformation.TransformAsync(result);
        return result;
    }
}

public class RealmRoleTransformation : IClaimsTransformation
{
    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
            return Task.FromResult(result);

        var realmAccess = principal.FindFirst("realm_access")?.Value;
        if (string.IsNullOrWhiteSpace(realmAccess))
            return Task.FromResult(result);

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var roleContainer = JsonSerializer.Deserialize<KeycloakJwtRoleContainer>(realmAccess, jsonSerializerOptions);
        if (roleContainer == null)
            return Task.FromResult(result);

        var clientRoles = roleContainer.Roles?
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .ToList()
            ?? new List<string>();

        foreach (var role in clientRoles)
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return Task.FromResult(result);
    }
}

public class ClientRoleTransformation : IClaimsTransformation
{
    private readonly string _clientId;

    public ClientRoleTransformation()
        => _clientId = "api";

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        var result = principal.Clone();
        if (result.Identity is not ClaimsIdentity identity)
            return Task.FromResult(result);

        var resourceAccessValue = principal.FindFirst("resource_access")?.Value;
        if (string.IsNullOrWhiteSpace(resourceAccessValue))
            return Task.FromResult(result);

        var jsonSerializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        var clients = JsonSerializer.Deserialize<KeycloakJwtClientRoles>(resourceAccessValue, jsonSerializerOptions);
        if (clients == null)
            return Task.FromResult(result);

        var clientRoleContainer = clients.FirstOrDefault(x => x.Key == _clientId);
        if (clientRoleContainer.Key == null)
            return Task.FromResult(result);

        var clientRoles = clientRoleContainer.Value.Roles?
            .Where(role => !string.IsNullOrWhiteSpace(role))
            ?? new List<string>();

        foreach (var role in clientRoles)
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));

        return Task.FromResult(result);
    }
}

public class KeycloakJwtRoleContainer
{
    public List<string>? Roles { get; set; }
}

public class KeycloakJwtClientRoles : Dictionary<string, KeycloakJwtRoleContainer>
{
}
```