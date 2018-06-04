using System;
using System.Collections.Generic;
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
    /// 每天定时导出Excel任务
    /// </summary>
    public class ExportDayJobService : IRecurringTask
    {
        private readonly IMenuService _menuService;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(ExportDayJobService));

        public ExportDayJobService(IMenuService menuService)
        {
            _menuService = menuService;
        }

        /// <summary>
        /// 是否启用此job
        /// </summary>
        public bool Enable => true;

        /// <summary>
        /// 每天凌晨一点执行
        /// </summary>
        public string CronExpression => Cron.Daily(1);

        /// <summary>
        /// job的唯一标识
        /// </summary>
        public string JobId => "JuCheap-Job-ExportDayJobService";

        /// <summary>
        /// 执行job的方法
        /// </summary>
        /// <param name="context"></param>
        [DisplayName("每天定时导出Excel任务")]
        public void Execute(PerformContext context)
        {
            context.WriteLine("开始导出Excel文件");
            //获取导出的数据
            var filePath = Task.Run(async () => await _menuService.Export()).ConfigureAwait(false).GetAwaiter().GetResult();
            context.WriteLine($"导出成功,文件路径:{filePath}");

            context.WriteLine("开始发送邮件");
            //发送邮件
            var mailInstance = MailBase.Instance;
            mailInstance.Subject = $"菜单报表-{DateTime.Now.ToString("yyyyMMddHHmmss")}";
            mailInstance.Body = "菜单报表";
            mailInstance.To = new List<string> { "359484089@qq.com" };
            mailInstance.Attachments = new List<string> { filePath };
            var success = Task.Run(async () => await mailInstance.Send()).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!success)
            {
                context.ErrorWriteLine("邮件发送失败");
                Log.Error($"邮件发送失败{DateTime.Now}");
            }
            else
            {
                context.WriteLine("邮件发送成功");
            }

            context.WriteLine("定时导出Excel任务执行完成");
        }
    }
}
