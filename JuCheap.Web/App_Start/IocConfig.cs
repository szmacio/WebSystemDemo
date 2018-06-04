using System.Reflection;
using System.Web.Mvc;
using AutoMapper;
using JuCheap.Infrastructure;
using JuCheap.Services;
using Mehdime.Entity;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using WebGrease.Css.Extensions;

namespace JuCheap.Web
{
    /// <summary>
    /// ioc配置
    /// </summary>
    public class IocConfig
    {
        public static void Register()
        {
            RegisterForMvc();
        }

        /// <summary>
        /// RegisterForMvc
        /// </summary>
        private static void RegisterForMvc()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();

            // Register your types, for instance:
            RegisterForWebApiProxyClient(container);

            // This is an extension method from the integration package.
            container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            // This is an extension method from the integration package as well.
            container.RegisterMvcIntegratedFilterProvider();

            container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }

        /// <summary>
        /// RegisterForWebApiProxyClient
        /// </summary>
        /// <param name="container"></param>
        private static void RegisterForWebApiProxyClient(Container container)
        {
            //dbcontext
            container.Register<IDbContextScopeFactory>(() => new DbContextScopeFactory(), Lifestyle.Scoped);

            //service
            var moduleInitializers = new ModuleInitializer[]
            {
                new JuCheapModuleInitializer()
            };

            moduleInitializers.ForEach(x => x.LoadIoc(container));

            //automapper
            container.Register<IConfigurationProvider>(AutoMapperConfig.GetMapperConfiguration, Lifestyle.Singleton);
            container.Register(() => AutoMapperConfig.GetMapperConfiguration().CreateMapper(), Lifestyle.Scoped);
        }
    }
}