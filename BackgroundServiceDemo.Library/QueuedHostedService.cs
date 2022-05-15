using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BackgroundServiceDemo.Library
{
    public class QueuedHostedService : BackgroundService
    {
        private readonly ILogger _logger;

        public IBackgroundTaskQueue TaskQueue { get;}

        public QueuedHostedService(IBackgroundTaskQueue taskQueue, ILoggerFactory loggerFactory)
        {
            TaskQueue = taskQueue;
            _logger = loggerFactory.CreateLogger<QueuedHostedService>();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Queued Hosted Service is starting!");
            
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await TaskQueue.TryDequeueAsync(stoppingToken);

                try
                {
                    await workItem(stoppingToken);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, $@"Error occurred executing {nameof(workItem)}");

                }
            }

            _logger.LogInformation("Queued Hosted Service is stoppig!");
        }
    }
}
