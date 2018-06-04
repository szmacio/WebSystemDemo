using System.Web.Mvc;
using JuCheap.Web.Filters;

namespace JuCheap.Web.Controllers
{
    [IgnoreRightFilter]
    [RoutePrefix("pages")]
    public class PagesController : Controller
    {
        // GET: Pages
        [Route("{id}")]
        public ActionResult Index(string id)
        {
            return View(id);
        }
    }
}