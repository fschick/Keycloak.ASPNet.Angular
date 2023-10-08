```csharp
builder.Services.AddSwaggerGen(options =>
{
    options
        .AddSecurityDefinition(
            JwtBearerDefaults.AuthenticationScheme,
            new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OpenIdConnect,
                OpenIdConnectUrl = new Uri($"http://localhost:8080/realms/asp-net-keycloak/.well-known/openid-configuration")
            }
        );

    options.OperationFilter<AuthorizationOperationFilter>();
});

public class AuthorizationOperationFilter : IOperationFilter
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
                [oauth2SecurityScheme] = new[] { JwtBearerDefaults.AuthenticationScheme }
            }
        };
    }
}
```