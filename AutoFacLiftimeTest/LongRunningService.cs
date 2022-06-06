using Autofac;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFacLiftimeTest
{
    public class LongRunningService : BackgroundService
    {
        private readonly ILogger<LongRunningService> _logger;
        private readonly ILifetimeScope _rootScope;
        private readonly IBackgroundWorkerQueue _queue;

        public LongRunningService(
            ILogger<LongRunningService> logger,
            ILifetimeScope scope,
            IBackgroundWorkerQueue queue)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _rootScope = scope ?? throw new ArgumentNullException(nameof(scope));
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                try
                {
                    using ILifetimeScope queueScope = _rootScope.BeginLifetimeScope();
                    var workItem = await _queue.DequeueAsync(stoppingToken);
                    
                    await workItem(queueScope, stoppingToken);
                }
                catch (Exception ex)
                {
                    // log something here
                    _logger.LogError(ex, "Failed to do work");
                }
            }
        }
    }
}
