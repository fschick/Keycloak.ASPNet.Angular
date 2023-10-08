```csharp
[Authorize(Roles = "Weather")]
[HttpGet(Name = "GetWeatherForecast")]
public IEnumerable<WeatherForecast> Get()
{
}
```