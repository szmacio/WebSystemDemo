using AutoMapper;
using JuCheap.Infrastructure;
using JuCheap.Services;
using WebGrease.Css.Extensions;

namespace JuCheap.Web
{
    /// <summary>
    /// AutoMapperConfig
    /// </summary>
    public class AutoMapperConfig
    {
        private static MapperConfiguration _mapperConfiguration;

        /// <summary>
        /// 
        /// </summary>
        public static void Register()
        {
            var moduleInitializers = new ModuleInitializer[]
            {
                new JuCheapModuleInitializer()
            };

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                moduleInitializers.ForEach(m => m.LoadAutoMapper(cfg));
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MapperConfiguration GetMapperConfiguration()
        {
            if(_mapperConfiguration == null)
                Register();

            return _mapperConfiguration;
        }
    }
}