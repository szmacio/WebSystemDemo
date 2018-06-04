using Hangfire.Server;

namespace JuCheap.Interfaces.Task
{
    /// <summary>
    /// hangfire任务接口
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="context"></param>
        void Execute(PerformContext context);
    }
}
