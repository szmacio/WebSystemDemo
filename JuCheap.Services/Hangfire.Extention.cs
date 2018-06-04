using Hangfire.Console;
using Hangfire.Server;

namespace JuCheap.Services
{
    /// <summary>
    /// Hangfire 扩展
    /// </summary>
    public static class HangfireExtention
    {
        /// <summary>
        /// 输出警告信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content"></param>
        public static void WarningWriteLine(this PerformContext context, string content)
        {
            context.SetTextColor(ConsoleTextColor.Yellow);
            context.WriteLine(content);
            context.ResetTextColor();
        }
        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="context"></param>
        /// <param name="content"></param>
        public static void ErrorWriteLine(this PerformContext context, string content)
        {
            context.SetTextColor(ConsoleTextColor.Red);
            context.WriteLine(content);
            context.ResetTextColor();
        }
    }
}
