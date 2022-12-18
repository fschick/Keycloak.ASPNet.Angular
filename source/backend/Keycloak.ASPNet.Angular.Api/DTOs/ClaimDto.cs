using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// A Claim is a statement about an entity by an Issuer.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Public DTO")]

public class ClaimDto
{
    /// <summary>
    /// Gets the claim type of the <see cref="ClaimDto"/>.
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Gets the claim value of the <see cref="ClaimDto"/>.
    /// </summary>
    public string Value { get; set; }
}