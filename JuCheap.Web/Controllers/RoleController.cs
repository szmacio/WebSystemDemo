using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using JuCheap.Web.Filters;
using JuCheap.Web.Models;

namespace JuCheap.Web.Controllers
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;

        public RoleController(IRoleService roleSvc,IMenuService menuService)
        {
            _roleService = roleSvc;
            _menuService = menuService;
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
            return View(new RoleDto());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            var model = await _roleService.FindAsync(id);
            return View(model);
        }

        /// <summary>
        /// 角色授权
        /// </summary>
        /// <returns></returns>
        public ActionResult Authen()
        {
            return View();
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AuthenMenuDatas()
        {
            var list = await _menuService.GetTrees();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色树
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AuthenRoleDatas()
        {
            var list = await _roleService.GetTreesAsync();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色下的菜单
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> AuthenRoleMenus(string id)
        {
            var list = await _menuService.GetMenusByRoleId(id);
            var menuIds = list?.Select(item => item.Id);
            return Json(menuIds, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取角色下的菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SetRoleMenus(List<RoleMenuDto> datas)
        {
            var result = new JsonResultModel<bool>();
            if (datas.AnyOne())
            {
                result.flag = await _roleService.SetRoleMenusAsync(datas);
            }
            else
            {
                result.msg = "请选择需要授权的菜单";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 清空该角色下的所有权限
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ClearRoleMenus(string id)
        {
            var result = new JsonResultModel<bool>
            {
                flag = await _roleService.ClearRoleMenusAsync(id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(RoleDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.AddAsync(dto);
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
        public async Task<ActionResult> Edit(RoleDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.UpdateAsync(dto);
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
                result.flag = await _roleService.DeleteAsync(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithPager(RoleFilters filters)
        {
            var result = await _roleService.SearchAsync(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}