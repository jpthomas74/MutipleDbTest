using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreEfTest.Controllers
{
    [Route("[controller]")]
    [ApiController]
 
    public class WeatherForecastController : ControllerBase
    {
 
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly Db Db;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, Db db)
        {
            _logger = logger;
            Db = db;
            //var c = db.Customers.FirstOrDefault();
        }
    

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet]
        [Route("getcustomers")]
        public IEnumerable<Customer> GetCustomers()
        {
            return Db.Customers;
        }

    }
}
