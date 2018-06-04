using System.Threading.Tasks;
using System.Web.Mvc;
using JuCheap.Interfaces;
using JuCheap.Web.Filters;

namespace JuCheap.Web.Controllers
{
    /// <summary>
    /// 系统管理
    /// </summary>
    public class SystemController : BaseController
    {
        private readonly IDatabaseInitService _databaseInitService;

        public SystemController(IDatabaseInitService databaseInitService)
        {
            _databaseInitService = databaseInitService;
        }

        /// <summary>
        /// 系统管理首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 重置路径码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> ReloadPathCode()
        {
            var success = await _databaseInitService.InitPathCode();
            return Json(success);
        }

        /// <summary>
        /// 重新加载Hangfire定时任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ReloadTasks()
        {
            Startup.LoadRecurringTasks();
            return Ok();
        }

        /// <summary>
        /// 初始化省市区数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IgnoreRightFilter]
        public async Task<JsonResult> ReInitAreas()
        {
            var success = await _databaseInitService.InitAreas();
            return Json(success);
        }
    }
}