using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using JuCheap.Interfaces;
using JuCheap.Interfaces.Task;

namespace JuCheap.Services.TaskServices
{
    /// <summary>
    /// 删除失效代理Ip
    /// </summary>
    public class IpProxyDeleteJobService : IRecurringTask
    {
        private readonly IProxyIpService _proxyIpService;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(IpProxyDeleteJobService));

        public IpProxyDeleteJobService(IProxyIpService proxyIpService)
        {
            _proxyIpService = proxyIpService;
        }

        /// <summary>
        /// 是否启用此job
        /// </summary>
        public bool Enable => true;

        /// <summary>
        /// 每周星期日执行
        /// </summary>
        public string CronExpression => Cron.Weekly(DayOfWeek.Sunday);

        /// <summary>
        /// job的唯一标识
        /// </summary>
        public string JobId => "JuCheap-Job-DeleteDisabledIps";

        /// <summary>
        /// 执行job的方法
        /// </summary>
        /// <param name="context"></param>
        [DisplayName("删除失效代理IP任务")]
        public void Execute(PerformContext context)
        {
            context.WriteLine("开始删除失效代理IP");

            var number =
                Task.Run(async () => await _proxyIpService.DeleteDisabledProxyIps())
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            context.WriteLine($"总共删除{number}条.");
        }
    }
}
