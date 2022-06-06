using Autofac;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AutoFacLiftimeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DoStuff : ControllerBase
    {
        private readonly ILogger<DoStuff> _logger;
        private readonly ITestDep _testDep;
        private readonly ILifetimeScope _container;
        private readonly IBackgroundWorkerQueue _workerQueue;

        public DoStuff(
            ILogger<DoStuff> logger,
            ITestDep testDep,
            ILifetimeScope container,
            IBackgroundWorkerQueue backgroundWorkerQueue)
        {
            _logger = logger;
            _testDep = testDep;
            _container = container;
            _workerQueue = backgroundWorkerQueue;
        }

        [HttpGet("wind")]
        public IActionResult Get()
        {
            _testDep.DoStuff();

            return Ok("Done");
        }

        [HttpGet("queue")]
        public IActionResult Queue()
        {
            _workerQueue.QueueBackgroundWorkItem(async (scope,vtoken) =>
            {
                // Create scoped dependencies here, do not create them outside the scope
                var testDep = scope.Resolve<ITestDep>();

                await testDep.DoStuff();
            });

            return Ok("Done");
        }
    }
}
