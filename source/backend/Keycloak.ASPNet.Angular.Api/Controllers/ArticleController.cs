using Keycloak.ASPNet.Angular.Api.DTOs;
using Keycloak.ASPNet.Angular.Api.Policies;
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
    /// Gets all articles
    /// </summary>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    /// <returns>
    /// The articles found
    /// </returns>
    [HttpGet]
    [Authorize(Policy = UmaPolicy.SCOPE_READ)]
    public ActionResult<ICollection<ArticleDto>> GetArticles(CancellationToken cancellationToken = default)
        => Ok(_articles.Values);

    /// <summary>
    /// Gets a single article
    /// </summary>
    /// <param name="id">An optional article identifier</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    /// <returns>
    /// The articles found
    /// </returns>
    [HttpGet]
    [Authorize(Policy = UmaPolicy.SCOPE_READ)]
    public ActionResult<ArticleDto> GetArticle([Required] string id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var articleFound = _articles.TryGetValue(id, out var article);
        if (!articleFound)
            return NotFound();

        return Ok(article);
    }

    /// <summary>
    /// Adds an article
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null</exception>
    /// <param name="article">The article</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    [HttpPost]
    [Authorize(Policy = UmaPolicy.SCOPE_MANAGE)]
    public ActionResult AddArticle([Required] ArticleDto article, CancellationToken cancellationToken = default)
    {
        if (article == null)
            throw new ArgumentNullException(nameof(article));

        var articleAdded = _articles.TryAdd(article.Id, article);
        if (!articleAdded)
            return Conflict();

        return Ok();
    }

    /// <summary>
    /// Removes an article
    /// </summary>
    /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null</exception>
    /// <param name="id">The identifier of the article to remove</param>
    /// <param name="cancellationToken">A token that allows processing to be cancelled</param>
    [HttpDelete]
    [Authorize(Policy = UmaPolicy.SCOPE_MANAGE)]
    public ActionResult RemoveArticle([Required] string id, CancellationToken cancellationToken = default)
    {
        if (id == null)
            throw new ArgumentNullException(nameof(id));

        var articleRemoved = _articles.TryRemove(id, out _);
        if (!articleRemoved)
            return NotFound();

        return Ok();
    }
}