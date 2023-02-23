using Microsoft.AspNetCore.Mvc;

namespace middlewaredenemev6.Controllers;

[ApiController]
[Route("api/[action]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }


    [HttpPost()]
    public IActionResult GetMethodDeneme([FromBody] DTOProcess weather)
    {
        try
        {
            Thread.Sleep(3 * 1000);
            var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
        .ToArray();
            return Ok(res);
        }
        catch (System.Exception ex)
        {

            throw ex;
        }


    }

    [HttpPost()]
    public IActionResult GetMethodWithGetError([FromBody] DTOProcess weather)
    {
        int s1 = 123;
        int s2 = 0;
        var asd = s1 / s2;

        try
        {

            var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
        .ToArray();
            return Ok(res);
        }
        catch (System.Exception ex)
        {

            throw ex;
        }


    }

}
