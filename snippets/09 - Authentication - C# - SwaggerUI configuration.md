```csharp
app.UseSwaggerUI(options =>
{
    options.OAuthClientId("api");
    options.OAuthUsePkce();
    options.EnablePersistAuthorization();
});
```