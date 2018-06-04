using Hangfire.Server;

namespace JuCheap.Interfaces.Task
{
    /// <summary>
    /// Hangfire Job循环执行任务接口
    /// </summary>
    public interface IRecurringTask
    {
        bool Enable { get; }

        /// <summary>
        /// Cron 表达式
        /// </summary>
        string CronExpression { get; }
        /// <summary>
        /// 唯一的JobId
        /// </summary>
        string JobId { get; }

        void Execute(PerformContext context);
    }
}
