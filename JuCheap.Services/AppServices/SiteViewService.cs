using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using JuCheap.Data;
using JuCheap.Data.Entity;
using JuCheap.Interfaces;
using JuCheap.Models;
using Mehdime.Entity;

namespace JuCheap.Services.AppServices
{
    public class SiteViewService : ISiteViewService
    {
        private readonly IDbContextScopeFactory _dbContextScopeFactory;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="dbContextScopeFactory"></param>
        public SiteViewService(IDbContextScopeFactory dbContextScopeFactory)
        {
            _dbContextScopeFactory = dbContextScopeFactory;
        }


        /// <summary>
        /// 添加或更新网站访问量
        /// </summary>
        /// <param name="day">日期</param>
        public async Task<bool> AddOrUpdate(DateTime day)
        {
            using (var scope = _dbContextScopeFactory.Create())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var date = day.Date;
                var endDate = date.AddDays(1);
                var number = await db.PageViews.CountAsync(x => x.CreateDateTime > date && x.CreateDateTime < endDate);
                if (await db.SiteViews.AnyAsync(x => x.Day == date))
                {
                    var view = await db.SiteViews.FirstOrDefaultAsync(x => x.Day == date);
                    if (view != null)
                    {
                        view.Number = number;
                    }
                }
                else
                {
                    var view = new SiteViewEntity
                    {
                        Day = date,
                        Number = number
                    };
                    view.Create();
                    db.SiteViews.Add(view);
                }
                await scope.SaveChangesAsync();
                return true;
            }
        }

        /// <summary>
        /// 获取指定时间段内的访问量统计数据
        /// </summary>
        /// <param name="from">开始时间</param>
        /// <param name="to">结束时间</param>
        /// <returns></returns>
        public async Task<IList<SiteViewDto>> GetSiteViews(DateTime from, DateTime to)
        {
            using (var scope = _dbContextScopeFactory.CreateReadOnly())
            {
                var db = scope.DbContexts.Get<JuCheapContext>();
                var start = from.Date.AddDays(-1);
                var end = to.Date.AddDays(1);
                return
                    await
                        db.SiteViews.Where(x => x.Day > start && x.Day < end)
                            .Select(x => new SiteViewDto {Day = x.Day, Number = x.Number})
                            .ToListAsync();
            }
        }
    }
}
