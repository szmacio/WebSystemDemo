using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JuCheap.Interfaces;
using log4net.Config;
using StackExchange.Profiling;
using StackExchange.Profiling.EntityFramework6;

namespace JuCheap.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly bool MiniProfileEnabled = bool.Parse(ConfigurationManager.AppSettings["MiniProfileEnabled"]);

        protected void Application_Start()
        {
            if (MiniProfileEnabled)
                MiniProfilerEF6.Initialize();

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            IocConfig.Register();
            ModelBinderConfig.RegisterModleBinders(ModelBinders.Binders);

            var logFilePath = $"{AppDomain.CurrentDomain.BaseDirectory}/Configs/log4net.config";
            XmlConfigurator.ConfigureAndWatch(new FileInfo(logFilePath));

            var dbInitService = DependencyResolver.Current.GetService<IDatabaseInitService>();
            dbInitService.Init();
        }

        protected void Application_BeginRequest()
        {
            if (MiniProfileEnabled)
                MiniProfiler.Start();
        }

        protected void Application_EndRequest()
        {
            if (MiniProfileEnabled)
                MiniProfiler.Stop();
        }
    }
}
