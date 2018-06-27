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
        private readonly ISinglePageService _singlePageService;
      
        // GET: SinglePageculturecontact
        public SinglePageController(ISinglePageService singlePageService)
        {
            _singlePageService = singlePageService;
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