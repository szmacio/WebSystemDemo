using AutoMapper;
using SimpleInjector;

namespace JuCheap.Infrastructure
{
    /// <summary>
    /// 模块初始化
    /// </summary>
    public abstract class ModuleInitializer
    {
        /// <summary>
        /// 加载SimpleInject配置
        /// </summary>
        /// <param name="container"></param>
        public abstract void LoadIoc(Container container);

        /// <summary>
        /// 加载AutoMapper配置
        /// </summary>
        /// <param name="cofig"></param>
        public abstract void LoadAutoMapper(IMapperConfigurationExpression cofig);
    }
}
