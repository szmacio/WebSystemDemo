using JuCheap.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JuCheap.Web.Controllers
{
    public class SinglePageController : BaseController
    {
        private readonly ILogService _logService;
        private readonly ISiteViewService _siteViewService;
        // GET: SinglePageculturecontact
        public SinglePageController(ILogService logService, ISiteViewService siteViewService)
        {
            _logService = logService;
            _siteViewService = siteViewService;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Introduction()
        {
            return View();
        }
        public ActionResult culture()
        {
            return View();
        }
        public ActionResult DownLoad()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}