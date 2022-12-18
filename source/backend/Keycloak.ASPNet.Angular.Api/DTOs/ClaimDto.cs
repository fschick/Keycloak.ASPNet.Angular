namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// A Claim is a statement about an entity by an Issuer.
/// </summary>
public class ClaimDto
{
    /// <summary>
    /// Gets the claim type of the <see cref="ClaimDto"/>.
    /// </summary>
    public string Type { get; init; }

    /// <summary>
    /// Gets the claim value of the <see cref="ClaimDto"/>.
    /// </summary>
    public string Value { get; init; }
}