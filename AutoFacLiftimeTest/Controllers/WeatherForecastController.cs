using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFacLiftimeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ITestDep _testDep;

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            ITestDep testDep)
        {
            _logger = logger;
            _testDep = testDep;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _testDep.DoStuff();

            return Ok("Done");
        }
    }
}
