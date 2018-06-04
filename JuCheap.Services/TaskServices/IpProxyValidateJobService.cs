using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Interfaces;
using JuCheap.Interfaces.Task;

namespace JuCheap.Services.TaskServices
{
    /// <summary>
    /// 代理Ip有效性验证
    /// </summary>
    public class IpProxyValidateJobService : IRecurringTask
    {
        private readonly IProxyIpService _proxyIpService;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(IpProxyValidateJobService));

        public IpProxyValidateJobService(IProxyIpService proxyIpService)
        {
            _proxyIpService = proxyIpService;
        }

        /// <summary>
        /// 是否启用此job
        /// </summary>
        public bool Enable => true;

        /// <summary>
        /// 每十分钟执行一次
        /// </summary>
        public string CronExpression => Cron.MinuteInterval(5);

        /// <summary>
        /// job的唯一标识
        /// </summary>
        public string JobId => "JuCheap-Job-IpProxyValidateJobService";

        /// <summary>
        /// 执行job的方法
        /// </summary>
        /// <param name="context"></param>
        [DisplayName("代理Ip有效性验证任务")]
        public void Execute(PerformContext context)
        {
            context.WriteLine("开始验证代理Ip的有效性");

            var ips =
                Task.Run(async () => await _proxyIpService.GetWaitValidateIps(20))
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            var total = ips.Count;
            var successCount = 0;
            var http = new HttpRequestProvider();
            foreach (var proxyIpDto in ips)
            {
                var success = false;
                try
                {
                    var result = http.Get("http://www.jucheap.com/home/showip", null, proxyIpDto.Ip, proxyIpDto.Port);
                    success = "ok".Equals(result, StringComparison.OrdinalIgnoreCase);
                }
                catch (Exception ex)
                {
                    context.WriteLine($"验证失败：{ex.Message}");
                }
                
                if (success)
                {
                    successCount++;
                }
                Task.Run(async () => await _proxyIpService.Update(proxyIpDto.Id, success)).ConfigureAwait(false);
            }
            context.WriteLine($"验证完成，总共{total}条，成功{successCount}条");
            context.WriteLine("验证执行完成");
        }
    }
}
