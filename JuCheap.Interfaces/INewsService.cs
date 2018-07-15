using System.Collections.Generic;
using System.Threading.Tasks;
using JuCheap.Infrastructure;
using JuCheap.Models;
using JuCheap.Models.Filters;

namespace JuCheap.Interfaces
{
    /// <summary>
    /// 日志契约
    /// </summary>
    public interface INewsService
    {

        /// <summary>
        /// 添加单页
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<string> Add(NewsInfoDto dto);
        Task<string> AddType(NewsTypeDto dto);
        /// <summary>
        /// 分页搜索
        /// </summary>
        /// <param name="filters">查询过滤参数</param>
        /// <returns></returns>
        Task<PagedResult<NewsTypeDto>> SearchType(PageFilter filters);
        Task<PagedResult<NewsInfoDto>> Search(PageFilter filters);
        /// <summary>
        /// 更新单页
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<bool> Update(NewsInfoDto dto);
        Task<bool> UpdateType(NewsTypeDto dto);
        /// <summary>
        /// 根据主键查询模型
        /// </summary>FindallNews
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<NewsInfoDto> Find(string id);
        Task<List<NewsInfoDto>> FindallNews();
        Task<NewsTypeDto> FindType(string id);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<string> ids);
        Task<List<NewsTypeDto>> GetType();


    }
}
