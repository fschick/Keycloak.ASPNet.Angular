using Keycloak.ASPNet.Angular.Api.DTOs;
using Keycloak.ASPNet.Angular.Api.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace Keycloak.ASPNet.Angular.Api.Controllers;

/// <summary>
/// Article API.
/// </summary>
[ApiV1Controller]
public class ArticleController : ControllerBase
{
    private static readonly ConcurrentDictionary<string, ArticleDto> _articles = new(
        new Dictionary<string, ArticleDto>
        {
            ["CuddlyToy"] = new() { Id = "CuddlyToy", Description = "A cuddly toy from your childhood" },
            ["CheapDiamonds"] = new() { Id = "CheapDiamonds", Description = "A few cheap plastic diamonds" }
        }
    );

    /// <summary>
    /// Creates an article
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null</exception>
    /// <param name="article">The article</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    [HttpPost]
    [Authorize]
    public ActionResult CreateArticle([Required] ArticleDto article, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(article);

        var articleAdded = _articles.TryAdd(article.Id, article);
        if (!articleAdded)
            return Conflict();

        return Ok();
    }

    /// <summary>
    /// Read all articles
    /// </summary>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    /// <returns>
    /// All articles
    /// </returns>
    [HttpGet]
    [Authorize]
    public ActionResult<ICollection<ArticleDto>> ReadArticles(CancellationToken cancellationToken = default)
        => Ok(_articles.Values);

    /// <summary>
    /// Reads an article by its identifier
    /// </summary>
    /// <param name="id">The identifier of the article to read</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    /// <returns>
    /// The requested article
    /// </returns>
    [HttpGet("{id}")]
    [Authorize]
    public ActionResult<ArticleDto> ReadArticle([Required] string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var articleFound = _articles.TryGetValue(id, out var article);
        if (!articleFound)
            return NotFound();

        return Ok(article);
    }

    /// <summary>
    /// Deletes an article
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null</exception>
    /// <param name="id">The identifier of the article to remove</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult DeleteArticle([Required] string id, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(id);

        var articleRemoved = _articles.TryRemove(id, out _);
        if (!articleRemoved)
            return NotFound();

        return Ok();
    }
}