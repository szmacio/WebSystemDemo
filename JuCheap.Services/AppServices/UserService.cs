using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Enum;
using JuCheap.Models.Filters;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    /// <summary>
    /// 用户实现类
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;
        private readonly IMapper _mapper;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        /// <param name="mapper"></param>
        public UserService(IDbContextScopeFactory dbContextScopeFactory, IMapper mapper)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
            _mapper = mapper;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> Add(UserAddDto user)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var entity = _mapper.Map<UserAddDto, UserEntity>(user);
                entity.Create();
                entity.Password = entity.Password.ToMd5();
                var db = scope.DbContexts.Get<JuCheapContext>();
                db.Users.Add(entity);

                await scope.SaveChangesAsync();
                return entity.Id;
            }
        }

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        public async Task<bool> Update(UserUpdateDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Users.LoadAsync(dto.Id);
                entity.LoginName = dto.LoginName;
                entity.RealName = dto.RealName;
                entity.Email = dto.Email;
                entity.DepartmentId = dto.DepartmentId;
                if (dto.Password.IsNotBlank())
                    entity.Password = dto.Password.ToMd5();
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        public async Task<UserDto> Find(string id)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Users.LoadAsync(id);
                var dto = _mapper.Map<UserEntity, UserDto>(entity);
                if (entity.Department != null)
                {
                    dto.DepartmentName = entity.Department.FullName;
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
                var entities = await db.Users.Where(x => ids.Contains(x.Id)).ToListAsync();
                entities.ForEach(x => x.IsDeleted = true);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns></returns>
        public async Task<UserLoginDto> Login(LoginDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var reslt = new UserLoginDto();
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = await db.Users.FirstOrDefaultAsync(x => x.LoginName == dto.LoginName.Trim());
                var loginLog = new LoginLogEntity
                {
                    Id = BaseIdGenerator.Instance.GetId(),
                    LoginName = dto.LoginName,
                    IP = dto.LoginIP
                };
                if (entity == null)
                {
                    reslt.Message = "账号不存在";
                    reslt.Result = LoginResult.AccountNotExists;
                    loginLog.UserId = "0";
                }
                else
                {
                    if (entity.Password == dto.Password.ToMd5())
                    {
                        reslt.LoginSuccess = true;
                        reslt.Message = "登陆成功";
                        reslt.Result = LoginResult.Success;
                        reslt.User = _mapper.Map<UserEntity, UserDto>(entity);
                    }
                    else
                    {
                        reslt.Message = "登陆密码错误";
                        reslt.Result = LoginResult.WrongPassword;
                    }
                    loginLog.UserId = entity.Id;
                }
                loginLog.Mac = reslt.Message;
                db.LoginLogs.Add(loginLog);
                await scope.SaveChangesAsync();
                return reslt;
            }
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> Give(string userId, string roleId)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var user = await db.Users.LoadAsync(userId);
                if (user.Roles.Any(x => x.Id == roleId))
                    return true;
                var role = await db.Roles.LoadAsync(roleId);
                user.Roles.Add(role);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 用户角色取消
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        public async Task<bool> Cancel(string userId, string roleId)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var user = await db.Users.LoadAsync(userId);
                var role = await db.Roles.LoadAsync(roleId);
                user.Roles.Remove(role);
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<bool> ResetPwd(string userId, string password)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
                if (user == null) return false;

                user.Password = password.ToMd5();
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        public async Task<bool> IsExists(string id, string loginName)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Users.Where(x => x.LoginName == loginName)
                    .WhereIf(id.IsNotBlank(), x => x.Id != id);

                return await query.AnyAsync();
            }
        }

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        public async Task<PagedResult<UserDto>> Search(UserFilters filters)
        {
            if (filters == null)
                return new PagedResult<UserDto>(1, 0);

            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Users
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.LoginName.Contains(filters.keywords) ||
                                                                 x.RealName.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(x => new UserDto
                    {
                        Id = x.Id,
                        LoginName = x.LoginName,
                        RealName = x.RealName,
                        Email = x.Email,
                        CreateDateTime = x.CreateDateTime,
                        DepartmentName = x.Department != null ? x.Department.FullName : string.Empty
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 是否拥有此权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        public async Task<bool> HasRight(string userId, string url)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.Menus.Include(x => x.Roles)
                    .Where(x => url.StartsWith(x.Url));
                var user = await db.Users.LoadAsync(userId);
                if (user.Roles.AnyOne())
                {
                    var userRoleIds = user.Roles.Select(x => x.Id);
                    return await query.AnyAsync(x => x.Roles.Any(r => userRoleIds.Contains(r.Id)));
                }
                //如果此用户没有任何角色，则就没有权限
                return false;
            }
        }

        /// <summary>
        /// 记录访问记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<bool> Visit(VisitDto dto)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var entity = _mapper.Map<VisitDto, PageViewEntity>(dto);
                entity.Create();
                db.PageViews.Add(entity);
                await scope.SaveChangesAsync();
                return true;
            }
        }
    }
}
