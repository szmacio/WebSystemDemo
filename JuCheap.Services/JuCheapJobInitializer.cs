using System.Linq;
using AutoMapper;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces.Task;
using SimpleInjector;

namespace JuCheap.Services
{
    /// <summary>
    /// Hangfire Job模块初始化
    /// </summary>
    public class JuCheapJobInitializer : ModuleInitializer
    {
        /// <summary>
        /// 加载SimpleInject配置
        /// </summary>
        /// <param name="container"></param>
        public override void LoadIoc(Container container)
        {
            var registrations =
                from type in typeof (JuCheapJobInitializer).Assembly.GetTypes()
                where
                    type.Namespace != null && (type.Namespace.IsNotBlank() &&
                                               type.Namespace.StartsWith("JuCheap.Services.TaskServices") &&
                                               type.GetInterfaces().Any(x => x == typeof(IRecurringTask)))
                select type;
            
            foreach (var jobType in registrations)
            {
                container.Register(jobType, jobType, Lifestyle.Scoped);
            }
        }

        /// <summary>
        /// 加载AutoMapper配置
        /// </summary>
        /// <param name="config"></param>
        public override void LoadAutoMapper(IMapperConfigurationExpression config)
        {
            
        }
    }
}
