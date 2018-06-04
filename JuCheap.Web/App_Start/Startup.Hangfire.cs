using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Hangfire;
using Hangfire.Console;
using Hangfire.MySql;
using Hangfire.SimpleInjector;
using Hangfire.SqlServer;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces.Task;
using Owin;
using SimpleInjector.Lifestyles;

namespace JuCheap.Web
{
    public partial class Startup
    {
        /// <summary>
        /// 配置Hangfire
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureHangfire(IAppBuilder app)
        {
            JobIocConfig.Register();

            //Hangfire IOC配置
            GlobalConfiguration.Configuration.UseActivator(new SimpleInjectorJobActivator(JobIocConfig.Container));

            //hangfire 配置
            var jobConnection = ConfigurationManager.ConnectionStrings["JuCheap-Job"];

            if (jobConnection.ProviderName.IsEqual("System.Data.SqlClient"))//使用SQLServer作为 数据库存储
            {
                var storage = new SqlServerStorage("JuCheap-Job");
                GlobalConfiguration.Configuration.UseStorage(storage).UseConsole();
            }
            else if (jobConnection.ProviderName.IsEqual("MySql.Data.MySqlClient"))//使用MySql作为 数据库存储
            {

                var storage = new MySqlStorage("JuCheap-Job");
                GlobalConfiguration.Configuration.UseStorage(storage).UseConsole();
            }

            GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

            //OWIN-based authentication
            var options = new DashboardOptions
            {
                Authorization = new[]
                {
                    new HangfireAuthorizationFilter()
                }
            };
            app.UseHangfireDashboard("/taskcenter", options);

            LoadRecurringTasks();

            var jobOptions = new BackgroundJobServerOptions
            {
                ServerName = Environment.MachineName
            };
            app.UseHangfireServer(jobOptions);
        }

        /// <summary>
        /// 加载Job
        /// </summary>
        public static void LoadRecurringTasks()
        {
            using (AsyncScopedLifestyle.BeginScope(JobIocConfig.Container))
            {
                var types = from type in Assembly.Load("JuCheap.Services").GetTypes()
                            where
                                type.Namespace?.StartsWith("JuCheap.Services.TaskServices") == true &&
                                type.GetInterfaces().Any(t => t == typeof(IRecurringTask))
                            select type;
                foreach (var type in types)
                {
                    var task = JobActivator.Current.ActivateJob(type) as IRecurringTask;
                    if (task == null)
                    {
                        continue;
                    }
                    if (task.Enable)
                    {
                        RecurringJob.AddOrUpdate(task.JobId, () => task.Execute(null), task.CronExpression, timeZone: TimeZoneInfo.Local);
                    }
                    else
                    {
                        RecurringJob.RemoveIfExists(task.JobId);
                    }
                }
            }
        }
    }
}