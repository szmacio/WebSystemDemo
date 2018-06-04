using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
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
    /// 日志契约实现
    /// </summary>
    public class LogService : ILogService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public LogService(IDbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
        }

        /// <summary>
        /// 获取登录日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public async Task<PagedResult<LoginLogDto>> SearchLoginLogsAsync(LogFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.LoginLogs
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.LoginName.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new LoginLogDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        LoginName = item.LoginName,
                        IP = item.IP,
                        Mac = item.Mac,
                        CreateDateTime = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取访问日志
        /// </summary>
        /// <param name="filters">过滤器</param>
        /// <returns></returns>
        public async Task<PagedResult<VisitDto>> SearchVisitLogsAsync(LogFilters filters)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = db.PageViews
                    .WhereIf(filters.keywords.IsNotBlank(), x => x.LoginName.Contains(filters.keywords));

                return await query.OrderByCustom(filters.sidx, filters.sord)
                    .Select(item => new VisitDto
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        LoginName = item.LoginName,
                        IP = item.IP,
                        Url = item.Url,
                        VisitDate = item.CreateDateTime
                    }).PagingAsync(filters.page, filters.rows);
            }
        }

        /// <summary>
        /// 获取最近七天的访问统计数据
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<VisitDataDto>> GetLastestVisitDataAsync()
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                const string fomart = "yyyy-MM-dd";
                var now = DateTime.Now;
                var date = new DateTime(now.Year, now.Month, now.Day).AddDays(-7);
                var db = scope.DbContexts.Get<JuCheapContext>();
                var query = await db.PageViews.Where(item => item.CreateDateTime >= date).ToListAsync();

                var result = (from item in query
                    group item by
                        new DateTime(item.CreateDateTime.Year, item.CreateDateTime.Month, item.CreateDateTime.Day)
                    into g
                    select new VisitDataDto
                    {
                        Date = g.Key.ToString("yyyy-MM-dd"),
                        Number = g.Count()
                    }).ToList();
                var datas = new List<VisitDataDto>();
                for (var i = 0; i < 7; i++)
                {
                    var currentDate = date.AddDays(i).ToString(fomart);
                    var data = result.FirstOrDefault(item => item.Date == currentDate);
                    if (data != null)
                        datas.Add(data);
                    datas.Add(new VisitDataDto { Date = currentDate, Number = 0 });
                }
                return datas;
            }
        }
    }
}
