using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
     // private static readonly string[] summaries = new[]
    // {
    //     "freezing", "bracing", "chilly", "cool", "mild", "warm", "balmy", "hot", "sweltering", "scorching"
    // };

    // private readonly ilogger<weatherforecastcontroller> _logger;

    // public weatherforecastcontroller(ilogger<weatherforecastcontroller> logger)
    // {
    //     _logger = logger;
    // }

    // [httpget(name = "getweatherforecast")]
    // public ienumerable<weatherforecast> get()
    // {
    //     return enumerable.range(1, 5).select(index => new weatherforecast
    //     {
    //         date = dateonly.fromdatetime(datetime.now.adddays(index)),
    //         temperaturec = random.shared.next(-20, 55),
    //         summary = summaries[random.shared.next(summaries.length)]
    //     })
    //     .toarray();
    // }
    

   

}


   