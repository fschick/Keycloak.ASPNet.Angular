```csharp
[Authorize("Weather#Read")]
[HttpGet(Name = "GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get()
{
}
```