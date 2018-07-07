using System.Linq;
using AutoMapper;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Models;
using JuCheap.Models.Enum;
using SimpleInjector;

namespace JuCheap.Services
{
    /// <summary>
    /// 模块初始化
    /// </summary>
    public class JuCheapModuleInitializer : ModuleInitializer
    {
        /// <summary>
        /// 加载SimpleInject配置
        /// </summary>
        /// <param name="container"></param>
        public override void LoadIoc(Container container)
        {
            var registrations =
                from type in typeof (JuCheapModuleInitializer).Assembly.GetTypes()
                where
                    type.Namespace != null && (type.Namespace.IsNotBlank() &&
                                               type.Namespace.StartsWith("JuCheap.Services.AppServices") &&
                                               type.GetInterfaces().Any(x => x.Name.EndsWith("Service")) &&
                                               type.GetInterfaces().Any())
                select new {Service = type.GetInterfaces().First(), Implementation = type};

            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation, Lifestyle.Scoped);
            }
        }

        /// <summary>
        /// 加载AutoMapper配置
        /// </summary>
        /// <param name="config"></param>
        public override void LoadAutoMapper(IMapperConfigurationExpression config)
        {
            config.CreateMap<UserEntity, UserDto>().ReverseMap();
            config.CreateMap<UserEntity, UserAddDto>().ReverseMap();
            config.CreateMap<SinglePageEntity, SinglePageDto>().ReverseMap();
            config.CreateMap<UserEntity, UserUpdateDto>().ReverseMap();
            config.CreateMap<UserDto, UserUpdateDto>().ReverseMap();
            config.CreateMap<RoleEntity, RoleDto>().ReverseMap();
            config.CreateMap<PageViewEntity, VisitDto>()
                .ForMember(v => v.VisitDate, e => e.MapFrom(pv => pv.CreateDateTime))
                .ReverseMap();
            config.CreateMap<LoginLogEntity, LoginLogDto>().ReverseMap();
            config.CreateMap<MenuEntity, MenuDto>()
                .ForMember(m => m.Type, e => e.MapFrom(item => (MenuType)item.Type))
                .ReverseMap();
            config.CreateMap<MenuEntity, TreeDto>()
                .ForMember(m => m.id, e => e.MapFrom(item => item.Id))
                .ForMember(m => m.pId, e => e.MapFrom(item => item.ParentId))
                .ForMember(m => m.name, e => e.MapFrom(item => item.Name));
            config.CreateMap<RoleEntity, TreeDto>()
                .ForMember(m => m.id, e => e.MapFrom(item => item.Id))
                .ForMember(m => m.name, e => e.MapFrom(item => item.Name));
            config.CreateMap<DepartmentEntity, DepartmentDto>().ReverseMap();
            config.CreateMap<AreaEntity, AreaDto>().ReverseMap();
        }
    }
}
