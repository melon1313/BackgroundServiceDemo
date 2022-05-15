namespace BackgroundServiceDemo.Library
{
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// add work item running at background
        /// </summary>
        /// <param name="workItem"></param>
        void AddBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        /// <summary>
        /// 嘗試移除並傳回位在並行佇列開頭的物件
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Func<CancellationToken, Task>> TryDequeueAsync(CancellationToken cancellationToken);
    }
}