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
    public interface ILogService
    {
        /// <summary>
        /// 获取登录日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        Task<PagedResult<LoginLogDto>> SearchLoginLogsAsync(LogFilters filters);

        /// <summary>
        /// 获取访问日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        Task<PagedResult<VisitDto>> SearchVisitLogsAsync(LogFilters filters);

        /// <summary>
        /// 获取最近七天的访问统计数据
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<VisitDataDto>> GetLastestVisitDataAsync();
    }
}
