using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AutoFacLiftimeTest
{
    public interface ITestDep : IDisposable
    {
        Task DoStuff();
    }

    public class TestDep : ITestDep
    {

        private readonly ILogger<TestDep> _logger;
        private bool _isDisposed;

        public TestDep(ILogger<TestDep> logger)
        {
            _logger = logger;
        }

        public void Dispose()
        {
            _logger.LogInformation("I've been disposed");
            _isDisposed = true;
        }

        public async Task DoStuff()
        {
            int delayTimeMs = 6000;
            _logger.LogInformation($"Waiting {delayTimeMs}ms");

            await Task.Delay(delayTimeMs);

            if (_isDisposed)
            {
                _logger.LogError("I'm already disposed");
            }

            _logger.LogInformation("Done waiting");
        }


        
    }
}
