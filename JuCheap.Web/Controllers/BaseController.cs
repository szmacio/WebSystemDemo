using System.Web.Mvc;

namespace JuCheap.Web.Controllers
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult Ok(object data = null)
        {
            return Json(new {flag = true, data}, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 成功,原样返回data的值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public JsonResult JsonOk(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="msg">提示消息</param>
        /// <returns></returns>
        public JsonResult Fail(string msg = "操作失败")
        {
            return Json(new { flag = false, msg }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// json
        /// </summary>
        /// <param name="success">是否操作成功</param>
        /// <returns></returns>
        public JsonResult Json(bool success)
        {
            var msg = success ? "操作成功" : "操作失败";
            return Json(new { flag = success, msg }, JsonRequestBehavior.AllowGet);
        }
    }
}