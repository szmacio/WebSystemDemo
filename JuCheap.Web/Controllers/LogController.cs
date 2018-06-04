using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using JuCheap.Web.Filters;
using JuCheap.Web.Models;

namespace JuCheap.Web.Controllers
{
    /// <summary>
    /// 日志
    /// </summary>
    public class LogController : BaseController
    {
        private readonly ILogService _logService;
        private readonly ISiteViewService _siteViewService;

        public LogController(ILogService logService, ISiteViewService siteViewService)
        {
            _logService = logService;
            _siteViewService = siteViewService;
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <returns></returns>
        public ActionResult Logins()
        {
            return View();
        }

        /// <summary>
        /// 访问记录
        /// </summary>
        /// <returns></returns>
        public ActionResult Visits()
        {
            return View();
        }

        /// <summary>
        /// 登录日志
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> LoginsList(LogFilters filters)
        {
            var result = await _logService.SearchLoginLogsAsync(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 访问记录
        /// </summary>
        /// <param name="filters"></param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> VisitsList(LogFilters filters)
        {
            var result = await _logService.SearchVisitLogsAsync(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 图表
        /// </summary>
        /// <returns></returns>
        public ActionResult Charts()
        {
            return View();
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> ChartsDatas()
        {
            var end = DateTime.Now.Date.AddDays(-1);
            var start = DateTime.Now.Date.AddDays(-7);
            var list = await _siteViewService.GetSiteViews(start, end);
            var result = new
            {
                categoryDatas = list.Select(x => x.Day.ToString("yyyy-MM-dd")),
                datas = list.Select(x => x.Number),
                pieDatas = list.Select(x=>new { value = x.Number,name=x.Day.ToString("yyyy-MM-dd")})
            };
            return JsonOk(result);
        }
    }
}