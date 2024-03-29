﻿using System.ComponentModel.DataAnnotations;

namespace Keycloak.ASPNet.Angular.Api.DTOs;

/// <summary>
/// A treasure.
/// </summary>
/// <autogeneratedoc />
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