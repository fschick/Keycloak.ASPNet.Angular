using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// An article.
/// </summary>
[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global", Justification = "Public DTO")]
public class ArticleDto
{
    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    [Required]
    public string Id { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    public string Description { get; set; }
}