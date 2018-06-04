using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Interfaces;
using JuCheap.Interfaces.Task;
using JuCheap.Models;

namespace JuCheap.Services.TaskServices
{
    /// <summary>
    /// 代理Ip抓取
    /// </summary>
    public class IpProxyGrabJobService : IRecurringTask
    {
        private readonly IProxyIpService _proxyIpService;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(IpProxyGrabJobService));

        public IpProxyGrabJobService(IProxyIpService proxyIpService)
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
        public string CronExpression => Cron.MinuteInterval(10);

        /// <summary>
        /// job的唯一标识
        /// </summary>
        public string JobId => "JuCheap-Job-IpProxyGrabJobService";

        /// <summary>
        /// 执行job的方法
        /// </summary>
        /// <param name="context"></param>
        [DisplayName("代理Ip抓取任务")]
        public void Execute(PerformContext context)
        {
            context.WriteLine("开始抓取代理Ip地址");

            var ip1 = GrabWest(context);
            var ip2 = GrabYun(context);
            var ip3 = GrabSea(context);
            var ip4 = Grab66(context);

            var proxyIp = new List<ProxyIpDto>();
            if (ip1.AnyOne())
                proxyIp.AddRange(ip1);
            if (ip2.AnyOne())
                proxyIp.AddRange(ip2);
            if (ip3.AnyOne())
                proxyIp.AddRange(ip3);
            if (ip4.AnyOne())
                proxyIp.AddRange(ip4);

            context.WriteLine("开始存储到数据库");
            Task.Run(async () => await _proxyIpService.AddIfNotExists(proxyIp)).ConfigureAwait(false);
            context.WriteLine("存储到数据库已完成");

            context.WriteLine("任务执行完成");
        }

        private List<ProxyIpDto> GrabWest(PerformContext context)
        {
            var proxyIp = new List<ProxyIpDto>();
            try
            {
                context.WriteLine("->西刺");
                var http = new HttpRequestProvider();
                var url = "http://www.xicidaili.com/nn/1"; // 西刺
                var result = http.Get(url);
                if (result.IsNotBlank())
                {
                    var count = 0;
                    var regex = @"<td>(\d+\.\d+\.\d+\.\d+)</td>\s+<td>(\d+)</td>";
                    var mstr = Regex.Match(result, regex);
                    while (mstr.Success)
                    {
                        var ip = new ProxyIpDto
                        {
                            Ip = mstr.Groups[1].Value,
                            Port = mstr.Groups[2].Value.ToInt()
                        };
                        proxyIp.Add(ip);
                        mstr = mstr.NextMatch();
                        count++;
                    }
                    context.WriteLine($"->西刺，抓取到{count}条");
                }
            }
            catch (Exception ex)
            {
                context.WriteLine($"->西刺，发生错误:{ex.Message}");
                Log.Error(ex);
            }
            return proxyIp;
        }

        private List<ProxyIpDto> GrabYun(PerformContext context)
        {
            var http = new HttpRequestProvider();
            var proxyIp = new List<ProxyIpDto>();
            context.WriteLine("->云代理");
            try
            {
                var url = "http://www.ip3366.net/free/?stype=1"; // 云代理
                var result = http.Get(url);
                if (result.IsNotBlank())
                {
                    var count = 0;
                    var regex = @"<td>(\d+\.\d+\.\d+\.\d+)</td>\s+<td>(\d+)</td>";
                    var mstr = Regex.Match(result, regex);
                    while (mstr.Success)
                    {
                        var ip = new ProxyIpDto
                        {
                            Ip = mstr.Groups[1].Value,
                            Port = mstr.Groups[2].Value.ToInt()
                        };
                        proxyIp.Add(ip);
                        mstr = mstr.NextMatch();
                        count++;
                    }
                    context.WriteLine($"->云代理，抓取到{count}条");
                }
            }
            catch (Exception ex)
            {
                context.WriteLine($"->云代理，发生错误:{ex.Message}");
                Log.Error(ex);
            }
            return proxyIp;
        }

        private List<ProxyIpDto> GrabSea(PerformContext context)
        {
            var http = new HttpRequestProvider();
            var proxyIp = new List<ProxyIpDto>();
            context.WriteLine("->IP海");
            try
            {
                var url = "http://www.iphai.com/free/ng"; // IP海
                var result = http.Get(url);
                if (result.IsNotBlank())
                {
                    var count = 0;
                    var regex = @"<td>\s+(\d+\.\d+\.\d+\.\d+)\s+</td>\s+<td>\s+(\d+)\s+</td>";
                    var mstr = Regex.Match(result, regex);
                    while (mstr.Success)
                    {
                        var ip = new ProxyIpDto
                        {
                            Ip = mstr.Groups[1].Value,
                            Port = mstr.Groups[2].Value.ToInt()
                        };
                        proxyIp.Add(ip);
                        mstr = mstr.NextMatch();
                        count++;
                    }
                    context.WriteLine($"->IP海，抓取到{count}条");
                }
            }
            catch (Exception ex)
            {
                context.WriteLine($"->IP海，发生错误:{ex.Message}");
                Log.Error(ex);
            }
            return proxyIp;
        }

        private List<ProxyIpDto> Grab66(PerformContext context)
        {
            var http = new HttpRequestProvider();
            var proxyIp = new List<ProxyIpDto>();
            context.WriteLine("->66Ip");
            try
            {
                var url = "http://www.66ip.cn/nmtq.php?getnum=100&isp=0&anonymoustype=3&start=&ports=&export=&ipaddress=&area=1&proxytype=2&api=66ip"; // 66ip
                var result = http.Get(url);
                if (result.IsNotBlank())
                {
                    var count = 0;
                    var regex = @"(\d+\.\d+\.\d+\.\d+):(\d+)";
                    var mstr = Regex.Match(result, regex);
                    while (mstr.Success)
                    {
                        var ip = new ProxyIpDto
                        {
                            Ip = mstr.Groups[1].Value,
                            Port = mstr.Groups[2].Value.ToInt()
                        };
                        proxyIp.Add(ip);
                        mstr = mstr.NextMatch();
                        count++;
                    }
                    context.WriteLine($"->66Ip，抓取到{count}条");
                }
            }
            catch (Exception ex)
            {
                context.WriteLine($"->66Ip，发生错误:{ex.Message}");
                Log.Error(ex);
            }
            return proxyIp;
        }
    }
}
