using System.Threading.Tasks;
using AutoMapper;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Services;
using Mehdime.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace JuCheap.Test
{
    [TestClass]
    public class ProjectTest
    {
        [TestMethod]
        public void AddProject()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Register your types, for instance:
            RegisterService(container);

            container.Verify();

            using (var scope = AsyncScopedLifestyle.BeginScope(container))
            {
                var service = scope.Container.GetInstance<IRoleService>();

                var role = new RoleDto
                {
                    Name = "test",
                    Description = "test"
                };

                var roleId = Task.Run(async () => await service.AddAsync(role)).ConfigureAwait(false).GetAwaiter().GetResult();

                Assert.IsTrue(roleId.IsNotBlank());
            }
        }

        /// <summary>
        /// RegisterForWebApiProxyClient
        /// </summary>
        /// <param name="container"></param>
        private static void RegisterService(Container container)
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
