using Autofac;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace AutoFacLiftimeTest
{
    public interface IBackgroundWorkerQueue
    {
        Task<Func<ILifetimeScope, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
        void QueueBackgroundWorkItem(Func<ILifetimeScope, CancellationToken, Task> workItem);
    }

    public class BackgroundWorkerQueue : IBackgroundWorkerQueue
    {
        private ConcurrentQueue<Func<ILifetimeScope, CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<ILifetimeScope, CancellationToken, Task>>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public async Task<Func<ILifetimeScope, CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);

            return workItem;
        }

        public void QueueBackgroundWorkItem(Func<ILifetimeScope, CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }
    }
}


