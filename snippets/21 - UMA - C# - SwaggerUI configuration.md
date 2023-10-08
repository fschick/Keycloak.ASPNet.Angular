```csharp
app.UseSwaggerUI(options =>
{
    options.OAuthClientId("frontend");
    
    options.InjectJavascript("/Extensions/RptInterceptor.js");
    var tokenUrl = $"http://localhost:8080/realms/asp-net-keycloak/protocol/openid-connect/token";
    var rptInterceptFunction = $"(req) {{ return entitlement(req, '{tokenUrl}', 'api'); }}";
    options.UseRequestInterceptor(rptInterceptFunction);
});

var environment = app.Services.GetRequiredService<IWebHostEnvironment>();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(environment.ContentRootPath, "Extensions")),
    RequestPath = $"/Extensions"
});
```