using AutoMapper;
using JuCheap.Interfaces;
using JuCheap.Models.Filters;
using JuCheap.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace JuCheap.Web.Controllers
{
    public class SinglePageController : BaseController
    {
        private readonly ISinglePageService _singlePageService;
        private readonly IMapper _mapper;

        // GET: SinglePageculturecontact
        public SinglePageController(ISinglePageService singlePageService,IMapper mapper)
        {
            _singlePageService = singlePageService;
            _mapper = mapper;
        }

        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithPager(PageFilter filters)
        {
            var result = await _singlePageService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
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