using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Enum;
using JuCheap.Models.Filters;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 菜单契约服务
    /// </summary>
    public class MenuService : IMenuService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public MenuService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public async Task<string> Add(MenuDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<MenuDto, MenuEntity>(dto);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();

                var existsCode = await db.Menus.Where(item => item.ParentId == dto.ParentId)
                    .Select(item => item.Code).ToListAsync();
                var pathCode = await db.PathCodes.FirstOrDefaultAsync(item => !existsCode.Contains(item.Code));
                entity.Code = pathCode.Code.Trim();
                if (entity.ParentId.IsNotBlank())
                {
                    var parent = await db.Menus.FindAsync(entity.ParentId);
                    entity.PathCode = string.Concat(parent.PathCode.Trim(), entity.Code.Trim());
                    entity.Type = parent.Type == 1 ? (byte)MenuType.Menu : (byte)MenuType.Button;
                }
                else
                {
                    entity.PathCode = entity.Code.Trim();
                    entity.Type = (byte)MenuType.Module;
                }
                db.Menus.Add(entity);

                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public async Task<bool> Update(MenuDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Menus.LoadAsync(dto.Id);
                entity.Name = dto.Name;
                entity.Url = dto.Url;
                entity.Order = dto.Order;
                entity.Icon = dto.Icon;
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<MenuDto> Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Menus.FindAsync(id);
                var dto = _mapper.Map<MenuEntity, MenuDto>(entity);
                if (dto.ParentId.IsNotBlank())
                {
                    var parent = await db.Menus.FindAsync(dto.ParentId);
                    if (parent != null)
                        dto.ParentName = parent.Name;
                }
                return dto;
            }
        }

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> Delete(IEnumerable<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entities = await db.Menus.Where(item => ids.Contains(item.Id)).ToListAsync();
                foreach (var menuEntity in entities)
                {
                    menuEntity.IsDeleted = true;
                }
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<MenuDto>> Search(MenuFilters filters)
        {
            if (filters == null)
                return new PagedResult<MenuDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Menus
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.Name.Contains(filters.keywords))
                    .WhereIf(filters.ExcludeType.HasValue, x => x.Type != (byte)filters.ExcludeType.Value);

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType)item.Type,
                        Icon = item.Icon,
                        CreateDateTime = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<MenuDto>> AdvanceSearch(AdvanceFilter filters)
        {
            if (filters == null)
                return new PagedResult<MenuDto>();

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var exp = filters.ToExpression<MenuEntity>();
                var query = db.Menus.WhereIf(exp != null, exp);

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType)item.Type,
                        CreateDateTime = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取用户拥有的权限菜单（不包含按钮）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<List<MenuDto>> GetMyMenus(string userId)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var user = db.Users.Load(userId);
                if (user.Roles.AnyOne())
                {
                    var roleIds = user.Roles.Select(x => x.Id);
                    var query = db.Menus.Where(x =>
                        x.Type != (byte) MenuType.Button && 
                        x.Roles.Any(r => roleIds.Contains(r.Id)));
                    return await query.OrderBy(x => x.Order).Select(item => new MenuDto
                    {
                        Id = item.Id,
                        ParentId = item.ParentId,
                        Name = item.Name,
                        Url = item.Url,
                        Order = item.Order,
                        Type = (MenuType) item.Type,
                        Icon = item.Icon
                    }).ToListAsync();
                }
                return null;
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeDto>> GetTrees()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var list = await db.Menus.ToListAsync();
                var result = _mapper.Map<List<MenuEntity>, List<TreeDto>>(list);
                result.ForEach(t => t.open = true);
                return result;
            }
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuDto>> GetMenusByRoleId(string roleId)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var list = await db.Menus.Where(m => m.Roles.Any(r => r.Id == roleId))
                    .ToListAsync();
                return _mapper.Map<List<MenuEntity>, List<MenuDto>>(list);
            }
        }

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<List<MenuDto>> QueryExportDatas(MenuFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Menus
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.Name.Contains(filters.keywords))
                    .WhereIf(filters.ExcludeType.HasValue, x => x.Type != (byte) filters.ExcludeType.Value);

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Url = item.Url
                    }).ToListAsync();
            }
        }

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> Export()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var list = await db.Menus.OrderBy(x => x.Order)
                    .Select(item => new MenuDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Url = item.Url
                    }).ToListAsync();
                return SpireHelper.SaveToExcel("菜单数据报表", list);
            }
        }
    }
}
