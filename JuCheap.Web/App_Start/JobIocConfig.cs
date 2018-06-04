using AutoMapper;
using JuCheap.Infrastructure;
using JuCheap.Services;
using JuCheap.Services.TaskServices;
using Mehdime.Entity;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;
using SimpleInjector.Lifestyles;
using WebGrease.Css.Extensions;

namespace JuCheap.Web
{
    /// <summary>
    /// Hangfire ioc配置
    /// </summary>
    public class JobIocConfig
    {
        public static Container Container { get; set; }

        /// <summary>
        /// RegisterForMvc
        /// </summary>
        public static void Register()
        {
            Container = new Container();
            Container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();
            InitializeContainer(Container);
            Container.Verify();
        }

        /// <summary>
        /// RegisterForWebApiProxyClient
        /// </summary>
        /// <param name="container"></param>
        private static void InitializeContainer(Container container)
        {
            //dbcontext
            container.Register<IDbContextScopeFactory>(() => new DbContextScopeFactory(), Lifestyle.Scoped);

            //service
            var moduleInitializers = new ModuleInitializer[]
            {
                new JuCheapModuleInitializer(),
                new JuCheapJobInitializer()
            };

            moduleInitializers.ForEach(x => x.LoadIoc(container));

            var updateJob = typeof(SystemUpdateJobService);
            container.Register(updateJob, updateJob, Lifestyle.Scoped);

            //automapper
            container.Register<IConfigurationProvider>(AutoMapperConfig.GetMapperConfiguration, Lifestyle.Singleton);
            container.Register(() => AutoMapperConfig.GetMapperConfiguration().CreateMapper(), Lifestyle.Scoped);
        }
    }
}