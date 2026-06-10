using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace TryUnitOfWork.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            var unitOfWorkFactory = new UnitOfWorkFactory<SqlConnection>("Data Source=esg-txcloud-new-asse-sqlsrv-d.privatelink.database.windows.net;Initial Catalog=txc_dev_tenant_gl;User ID=txc-dev-admin;Password=fUjRkDeX8LDe4pC3;MultipleActiveResultSets=true");
            var db = new DbContext(unitOfWorkFactory);

            Product product = null;

            try
            {
                product = db.Product.ReadAsync(1).Result;
                db.Commit();
            }
            catch (SqlException ex)
            {
                //log exception
                db.Rollback();
            }


            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}