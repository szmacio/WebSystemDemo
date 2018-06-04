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
using JuCheap.Models.Filters;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 角色契约实现
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public RoleService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="dto">角色模型</param>
        /// <returns></returns>
        public async Task<string> AddAsync(RoleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<RoleDto, RoleEntity>(dto);
                entity.Create();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.Roles.Add(entity);
                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="dto">角色模型</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(RoleDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = db.Roles.Find(dto.Id);
                _mapper.Map(dto, entity);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<RoleDto> FindAsync(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Roles.FindAsync(id);
                return _mapper.Map<RoleEntity, RoleDto>(entity);
            }
        }

        /// <summary>
        /// 批量逻辑删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(IEnumerable<string> ids)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entities = db.Roles.Where(item => ids.Contains(item.Id));
                entities.ForEach(item => item.IsDeleted = true);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<RoleDto>> SearchAsync(RoleFilters filters)
        {
            if (filters == null)
                return new PagedResult<RoleDto>(1, 15);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Roles
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.Name.Contains(filters.keywords));

                if (filters.UserId.IsNotBlank())
                {
                    var user = await db.Users.LoadAsync(filters.UserId);
                    var myRoleIds = user.Roles
                                    .Select(item => item.Id)
                                    .ToList();
                    query = filters.ExcludeMyRoles
                        ? query.Where(item => !myRoleIds.Contains(item.Id))
                        : query.Where(item => myRoleIds.Contains(item.Id));
                }

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new RoleDto
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Description = item.Description
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <returns></returns>
        public async Task<List<TreeDto>> GetTreesAsync()
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var list = await db.Roles.ToListAsync();
                return _mapper.Map<List<RoleEntity>, List<TreeDto>>(list);
            }
        }

        /// <summary>
        /// 设置角色权限
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SetRoleMenusAsync(List<RoleMenuDto> datas)
        {
            if (!datas.AnyOne()) return false;

            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var roleId = datas.First().RoleId;
                var role = await db.Roles.LoadAsync(roleId);
                var olds = role.Menus.ToList();
                var oldIds = olds.Select(item => item.Id);
                var newIds = datas.Select(item => item.MenuId);
                var adds = datas.Where(item => !oldIds.Contains(item.MenuId)).Select(x => x.MenuId).ToList();
                var removes = olds.Where(item => !newIds.Contains(item.Id)).ToList();
                if (adds.AnyOne())
                {
                    var addPages = await db.Menus.Where(x => adds.Contains(x.Id)).ToListAsync();
                    addPages.ForEach(m => role.Menus.Add(m));
                }
                if (removes.AnyOne())
                {
                    foreach (var menu in removes)
                    {
                        role.Menus.Remove(menu);
                    }
                }

                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 清空该角色下的所有权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> ClearRoleMenusAsync(string roleId)
        {
            if (roleId.IsBlank()) return false;

            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var role = db.Roles.Load(roleId);
                role.Menus = null;
                await scope.SaveChangesAsync();
                return true;
            }
        }
    }
}
