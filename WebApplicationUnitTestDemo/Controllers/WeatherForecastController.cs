using Microsoft.AspNetCore.Mvc;

using WebApplicationUnitTestDemo.Repositories;

namespace WebApplicationUnitTestDemo.Controllers;

[ApiController]
public class WeatherForecastController(
    IReservedRepository reservedRepository) : ControllerBase
{

    [HttpGet]
    [Route("/")]
    public IEnumerable<WeatherForecast> Get()
    {
        return [.. Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = reservedRepository.GetSummary("123")
        })];
    }
}
