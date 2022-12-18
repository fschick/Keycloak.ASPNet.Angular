using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;

namespace Keycloak.ASPNet.Angular.Api.Filters;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global", Justification = "Instantiated via DI.")]
internal class AuthorizationOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.DeclaringType == null)
            return;

        var authorizationRequired = context.MethodInfo.GetCustomAttributes(true)
            .Union(context.MethodInfo.DeclaringType.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>()
            .Any();

        if (!authorizationRequired)
            return;

        operation.Responses.Add(StatusCodes.Status401Unauthorized.ToString(), new OpenApiResponse { Description = nameof(HttpStatusCode.Unauthorized) });
        operation.Responses.Add(StatusCodes.Status403Forbidden.ToString(), new OpenApiResponse { Description = nameof(HttpStatusCode.Forbidden) });

        var oauth2SecurityScheme = new OpenApiSecurityScheme()
        {
            Reference = new OpenApiReference
            {
                Id = JwtBearerDefaults.AuthenticationScheme,
                Type = ReferenceType.SecurityScheme,
            },
        };

        operation.Security = new List<OpenApiSecurityRequirement> {
            new()
            {
                [oauth2SecurityScheme] = [JwtBearerDefaults.AuthenticationScheme]
            }
        };
    }
}