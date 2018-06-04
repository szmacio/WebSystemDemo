using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Hangfire;
using Hangfire.Console;
using Hangfire.Server;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Infrastructure.Utilities;
using JuCheap.Interfaces;
using JuCheap.Interfaces.Task;
using JuCheap.Models;
using JuCheap.Models.Enum;
using JuCheap.Services;
using JuCheap.Services.TaskServices;
using JuCheap.Web.Filters;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SimpleInjector.Lifestyles;

namespace JuCheap.Web.Controllers
{
    /// <summary>
    /// 首页
    /// </summary>
    [IgnoreRightFilter]
    public class HomeController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMenuService _menuService;
        private readonly IEmailSubscribeService _emailSubscribeService;

        public HomeController(IUserService userSvr, IMenuService menuService, IEmailSubscribeService emailSubscribeService)
        {
            _userService = userSvr;
            _menuService = menuService;
            _emailSubscribeService = emailSubscribeService;
        }

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            var myMenus = await _menuService.GetMyMenus(User.Identity.GetLoginUserId());
            return View(myMenus);
        }

        /// <summary>
        /// demo
        /// </summary>
        public ActionResult Report()
        {
            return View();
        }

        /// <summary>
        /// 工作流
        /// </summary>
        /// <returns></returns>
        public ActionResult GooFlow()
        {
            return View();
        }

        /// <summary>
        /// 欢迎页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Welcome()
        {
            return View();
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login()
        {
            var model = new LoginDto
            {
                ReturnUrl = Request.QueryString["ReturnUrl"],
                LoginName = "admin",
                Password = "qwaszx"
            };
            if (User.Identity.IsAuthenticated)
            {
                if (model.ReturnUrl.IsNotBlank())
                    return Redirect(model.ReturnUrl);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return View(model);
            model.LoginIP = HttpContext.Request.UserHostAddress;
            var loginDto = await _userService.Login(model);
            if (loginDto.LoginSuccess)
            {
                var authenticationManager = HttpContext.GetOwinContext().Authentication;
                var claims = new List<Claim>
                {
                    new Claim("LoginUserId", loginDto.User.Id),
                    new Claim(ClaimTypes.Name, model.LoginName)
                };
                var identity = new ClaimsIdentity(claims, DefaultAuthenticationTypes.ApplicationCookie);
                var pro = new AuthenticationProperties()
                {
                    IsPersistent = true
                };
                authenticationManager.SignIn(pro, identity);
                if (model.ReturnUrl.IsBlank())
                    return RedirectToAction("Index");
                return Redirect(model.ReturnUrl);
            }
            ModelState.AddModelError(loginDto.Result == LoginResult.AccountNotExists ? "LoginName" : "Password",
                loginDto.Message);
            return View(model);
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public string ShowIp()
        {
            return "ok";
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Subscribe(string email)
        {
            if (email.IsEmail())
            {
                await _emailSubscribeService.Subscribe(email);
                BackgroundJob.Enqueue(() => SendSubscribeEmail(null, email));
                return Ok();
            }
            return Fail("邮箱地址不正确");
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        /// <param name="email">邮箱地址</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> UnSubscribe(string email)
        {
            if (email.IsEmail())
            {
                await _emailSubscribeService.UnSubscribe(email);
                BackgroundJob.Enqueue(() => SendUnSubscribeEmail(null, email));
                return Ok();
            }
            return Fail("邮箱地址不正确");
        }

        [DisplayName("订阅成功邮件推送任务")]
        public async Task SendSubscribeEmail(PerformContext context, string email)
        {
            var name = email.Split('@')[0];
            context.WriteLine($"正在发送邮件给：{name}发送订阅成功的邮件");
            var sender = MailBase.Instance;
            sender.Subject = "JuCheap后台管系统框架更新消息订阅成功";
            sender.Body = "Hi,body:<br/>&nbsp;&nbsp;&nbsp;&nbsp;您已成功订阅JuCheap系统更新消息，如有更新，您会第一时间收到邮件通知.";
            sender.To = new List<string> { email };
            var success = await sender.Send();
            if (success)
            {
                context.WriteLine("邮件发送成功");
            }
            else
            {
                context.ErrorWriteLine("邮件发送失败");
            }
        }

        [DisplayName("取消订阅邮件推送任务")]
        public async Task SendUnSubscribeEmail(PerformContext context, string email)
        {
            var name = email.Split('@')[0];
            context.WriteLine($"正在发送邮件给：{name}发送取消订阅的邮件");
            var sender = MailBase.Instance;
            sender.Subject = "取消订阅JuCheap后台管系统框架更新消息";
            sender.Body = "Hi,body:<br/>&nbsp;&nbsp;&nbsp;&nbsp;您已成功取消订阅，此后，你将不会收到任何JuCheap系统更新的邮件通知消息.";
            sender.To = new List<string> { email };
            var success = await sender.Send();
            if (success)
            {
                context.WriteLine("邮件发送成功");
            }
            else
            {
                context.ErrorWriteLine("邮件发送失败");
            }
        }

        /// <summary>
        /// 推送更新
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PushSystemUpdateEmail()
        {
            if (User.Identity.Name == "jucheap")
            {
                using (AsyncScopedLifestyle.BeginScope(JobIocConfig.Container))
                {
                    var task = JobActivator.Current.ActivateJob(typeof(SystemUpdateJobService)) as IJob;
                    if (task != null)
                    {
                        BackgroundJob.Enqueue(() => task.Execute(null));
                        return Ok();
                    }
                    return Content("任务激活失败");
                }
            }
            return Content("你没有权限");
        }
    }
}