using System.Collections.Concurrent;

namespace BackgroundServiceDemo.Library
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new ConcurrentQueue<Func<CancellationToken, Task>>();   
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void AddBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if(workItem is null) throw new ArgumentNullException(nameof(workItem));

            _workItems.Enqueue(workItem);
            _signal.Release();
        }

        public async Task<Func<CancellationToken, Task>> TryDequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);

            _workItems.TryDequeue(out var workItem);

            return workItem;
        }
    }
}