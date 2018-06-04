using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using JuCheap.Infrastructure;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Enum;
using JuCheap.Models.Filters;
using JuCheap.Web.Filters;
using JuCheap.Web.Models;

namespace JuCheap.Web.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuController : BaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuSvc)
        {
            _menuService = menuSvc;
        }

        [AllowAnonymous]
        [IgnoreRightFilter]
        public ActionResult List()
        {
            return View();
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetSubGridListWithPager(MenuFilters filter)
        {
            var result = await _menuService.Search(filter);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View(new MenuDto());
        }

        /// <summary>
        /// 图标选择
        /// </summary>
        /// <returns></returns>
        [IgnoreRightFilter]
        public ActionResult FontAwesome()
        {
            return View();
        }

        /// <summary>
        /// Ajax模式添加菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [IgnoreRightFilter]
        public ActionResult AddAjax()
        {
            return View();
        }

        /// <summary>
        /// Ajax模式添加菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [IgnoreRightFilter]
        public async Task<ActionResult> AddAjax(MenuDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _menuService.Add(dto);
                if (result.IsNotBlank())
                    return Ok(result);
            }
            return Fail("验证失败");
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _menuService.Find(id);
            return View(model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(MenuDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _menuService.Add(dto);
                if (result.IsNotBlank())
                    return RedirectToAction("Index");
            }
            return View(dto);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(MenuDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _menuService.Update(dto);
                if (result)
                    return RedirectToAction("Index");
            }
            return View(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<JsonResult> Delete(IList<string> ids)
        {
            var result = new JsonResultModel<bool>();
            if (ids.AnyOne())
            {
                result.flag = await _menuService.Delete(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filter">查询参数</param>
        /// <param name="advanceFilter">高级查询</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithPager(MenuFilters filter, AdvanceFilter advanceFilter)
        {
            var result = await _menuService.Search(filter);
            //var result = _menuService.AdvanceSearch(advanceFilter);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="condition">查询参数</param>
        public async void Index_Exports(MenuFilters condition)
        {
            var menus = await _menuService.QueryExportDatas(condition);
            SpireHelper.ExportToExcel("菜单", menus);
        }
        /// <summary>
        /// 导出到Pdf
        /// </summary>
        /// <param name="condition">查询参数</param>
        public async void Index_Exports_Pdf(MenuFilters condition)
        {
            var menus = await _menuService.QueryExportDatas(condition);
            SpireHelper.ExportToPdf("菜单", menus);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="condition">查询参数</param>
        public async void Index_Print(MenuFilters condition)
        {
            var menus = await _menuService.QueryExportDatas(condition);
            SpireHelper.ExportToPdf("菜单", menus, true);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithKeywords(MenuFilters filters)
        {
            filters.page = 1;
            filters.rows = 10;
            filters.ExcludeType = MenuType.Button;
            var result = await _menuService.Search(filters);
            return Json(new { value = result.rows }, JsonRequestBehavior.AllowGet);
        }
    }
}