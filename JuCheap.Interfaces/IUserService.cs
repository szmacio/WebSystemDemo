﻿using System.Collections.Generic;
using System.Threading.Tasks;
using JuCheap.Infrastructure;
using JuCheap.Models;
using JuCheap.Models.Filters;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 用户契约
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<string> Add(UserAddDto dto);

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<bool> Update(UserUpdateDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<UserDto> Find(string id);

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="dto">登录信息</param>
        /// <returns></returns>
        Task<UserLoginDto> Login(LoginDto dto);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<string> ids);

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        Task<bool> Give(string userId, string roleId);

        /// <summary>
        /// 用户角色取消
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        Task<bool> Cancel(string userId, string roleId);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<bool> ResetPwd(string userId, string password);

        /// <summary>
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="loginName">登录名</param>
        /// <returns></returns>
        Task<bool> IsExists(string id, string loginName);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<PagedResult<UserDto>> Search(UserFilters filters);

        /// <summary>
        /// 是否拥有此权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="url">url地址</param>
        /// <returns></returns>
        Task<bool> HasRight(string userId, string url);

        /// <summary>
        /// 记录访问记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> Visit(VisitDto dto);
    }
}
