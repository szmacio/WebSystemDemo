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
    public interface ISinglePageService
    {

        /// <summary>
        /// 添加单页
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<string> Add(SinglePageDto dto);

        /// <summary>
        /// 更新单页
        /// </summary>
        /// <param name="dto">用户模型</param>
        /// <returns></returns>
        Task<bool> Update(SinglePageDto dto);

        /// <summary>
        /// 根据主键查询模型
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<SinglePageDto> Find(string id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="ids">主键ID集合</param>
        /// <returns></returns>
        Task<bool> Delete(IEnumerable<string> ids);


    }
}
