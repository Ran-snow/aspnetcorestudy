using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using MediatRTest.CommandMsg;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MediatRTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            UserAddMsg userAdd = new UserAddMsg();
            userAdd.Name = "MU_KENG";

            UserAddSuccessMsg userAddSuccessMsg = new UserAddSuccessMsg();
            userAddSuccessMsg.Message = "HAHADA";

            int res = await _mediator.Send(userAdd);

            _logger.LogInformation("_mediator.Send->{0}", res);

            _logger.LogInformation("---------------------------------");

            _logger.LogInformation("_mediator.Publish->{0} 开始", userAddSuccessMsg.Message);
            await _mediator.Publish(userAddSuccessMsg);
            _logger.LogInformation("_mediator.Publish->{0} 完成", userAddSuccessMsg.Message);

            var rng = new Random();
            return await Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray());
        }
    }
}
