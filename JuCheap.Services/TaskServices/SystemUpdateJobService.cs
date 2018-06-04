using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Hangfire.Console;
using Hangfire.Server;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Interfaces;
using JuCheap.Interfaces.Task;

namespace JuCheap.Services.TaskServices
{
    /// <summary>
    /// 系统更新，推送邮件给订阅人员
    /// </summary>
    public class SystemUpdateJobService : IJob
    {
        private readonly IEmailSubscribeService _emailSubscribeService;

        public SystemUpdateJobService(IEmailSubscribeService emailSubscribeService)
        {
            _emailSubscribeService = emailSubscribeService;
        }


        [DisplayName("系统更新邮件推送任务")]
        public void Execute(PerformContext context)
        {
            var emails =
                Task.Run(async () => await _emailSubscribeService.GetAll())
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();
            if (emails.AnyOne())
            {
                var number = emails.Count;
                context.WriteLine($"共获取到订阅者{number}位");
                var sender = MailBase.Instance;
                sender.Subject = "JuCheap后台管系统框架更新通知";
                sender.Body = "Hi,all:<br/>&nbsp;&nbsp;&nbsp;&nbsp;JuCheap系统有更新，详情请登录http://www.jucheap.com";
                sender.To = emails.ToList();
                var success = Task.Run(() => sender.Send()).ConfigureAwait(false).GetAwaiter().GetResult();
                context.WriteLine(success ? "邮件发送成功" : "邮件发送失败");
            }
            context.WriteLine("没有订阅人信息");
        }
    }
}
