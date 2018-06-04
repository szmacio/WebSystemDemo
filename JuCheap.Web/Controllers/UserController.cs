using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using AutoMapper;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using JuCheap.Web.Filters;
using JuCheap.Web.Models;

namespace JuCheap.Web.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserController(IUserService userSvc, IMapper mapper, IRoleService roleSvc)
        {
            _userService = userSvc;
            _mapper = mapper;
            _roleService = roleSvc;
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
        /// 用户角色授权
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public ActionResult Authen(string id)
        {
            return View();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View(new UserAddDto());
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            var dto = await _userService.Find(id);
            var model = _mapper.Map<UserDto, UserUpdateDto>(dto);
            return View(model);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Add(UserAddDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Add(dto);
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
        public async Task<ActionResult> Edit(UserUpdateDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.Update(dto);
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
                result.flag = await _userService.Delete(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 搜索页面
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithPager(UserFilters filters)
        {
            var result = await _userService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我的角色
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetMyRoles(RoleFilters filters)
        {
            var result = await _roleService.SearchAsync(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我尚未拥有的角色
        /// </summary>
        /// <param name="filters">查询参数</param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> GetNotMyRoles(RoleFilters filters)
        {
            filters.ExcludeMyRoles = true;
            var result = await _roleService.SearchAsync(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户角色授权
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/giveRight/{id}/{userId}")]
        public async Task<JsonResult> GiveRight(string id, string userId)
        {
            var result = new JsonResultModel<bool>
            {
                flag = await _userService.Give(userId, id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户角色取消
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        [Route("user/cancelRight/{id}/{userId}")]
        public async Task<JsonResult> CancelRight(string id, string userId)
        {
            var result = new JsonResultModel<bool>
            {
                flag = await _userService.Cancel(userId, id)
            };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetPwd(string id)
        {
            return View(new ResetPasswordDto {UserId = id});
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> ResetPwd(ResetPasswordDto model)
        {
            if (ModelState.IsValid)
            {
                await _userService.ResetPwd(model.UserId, model.Password);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        /// <summary>
        /// 判断登录名是否存在
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        [IgnoreRightFilter]
        public async Task<JsonResult> IsExists(string Id, string LoginName)
        {
            var exists = await _userService.IsExists(Id, LoginName);
            return JsonOk(!exists);
        }
    }
}