
using System.Collections.Generic;
using System.Threading.Tasks;
using JuCheap.Infrastructure;
using JuCheap.Models;
using JuCheap.Models.Filters;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 菜单契约
    /// </summary>
    public interface IMenuService
    {
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        Task<string> Add(MenuDto dto);

        /// <summary>
        /// 更新菜单
        /// </summary>
        /// <param name="dto">菜单模型</param>
        /// <returns></returns>
        Task<bool> Update(MenuDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<MenuDto> Find(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<string> ids);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<PagedResult<MenuDto>> Search(MenuFilters filters);

        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<PagedResult<MenuDto>> AdvanceSearch(AdvanceFilter filters);

        /// <summary>
        /// 获取用户拥有的权限菜单（不包含按钮）
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Task<List<MenuDto>> GetMyMenus(string userId);

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        Task<List<TreeDto>> GetTrees();

        /// <summary>
        /// 通过角色ID获取拥有的菜单权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        Task<List<MenuDto>> GetMenusByRoleId(string roleId);

        /// <summary>
        /// 获取导出数据
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<List<MenuDto>> QueryExportDatas(MenuFilters filters);

        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        Task<string> Export();
    }
}
