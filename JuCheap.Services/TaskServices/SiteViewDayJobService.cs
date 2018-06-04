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
    /// 每天定时统计网站访问量任务
    /// </summary>
    public class SiteViewDayJobService : IRecurringTask
    {
        private readonly ISiteViewService _siteViewService;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SiteViewDayJobService));

        public SiteViewDayJobService(ISiteViewService siteViewService)
        {
            _siteViewService = siteViewService;
        }

        /// <summary>
        /// 是否启用此job
        /// </summary>
        public bool Enable => true;

        /// <summary>
        /// 每天00：10执行
        /// </summary>
        public string CronExpression => Cron.Daily(0, 10);

        /// <summary>
        /// job的唯一标识
        /// </summary>
        public string JobId => "JuCheap-Job-SiteViewDayJobService";

        /// <summary>
        /// 执行job的方法
        /// </summary>
        /// <param name="context"></param>
        [DisplayName("每天定时统计网站访问量任务")]
        public void Execute(PerformContext context)
        {
            var day = DateTime.Now.AddDays(-1).Date;
            context.WriteLine($"开始统计{day.ToString("yyyy-MM-dd")}网站访问量");
            
            var success = Task.Run(async () => await _siteViewService.AddOrUpdate(day)).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!success)
            {
                context.ErrorWriteLine("统计失败");
            }
            else
            {
                context.WriteLine("统计完成");
            }
        }
    }
}
