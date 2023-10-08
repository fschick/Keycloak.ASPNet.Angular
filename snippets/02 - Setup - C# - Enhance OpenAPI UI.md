```csharp
app.UseSwaggerUI(options =>
{
    options.DisplayRequestDuration();
    options.EnableDeepLinking();
    options.EnableTryItOutByDefault();
    options.ConfigObject.AdditionalItems.Add("requestSnippetsEnabled", true);
});
```