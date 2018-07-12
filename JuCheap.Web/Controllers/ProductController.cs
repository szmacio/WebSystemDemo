using AutoMapper;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Interfaces;
using JuCheap.Models;
using JuCheap.Models.Filters;
using JuCheap.Web.Filters;
using JuCheap.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;


namespace JuCheap.Web.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        // GET: SinglePageculturecontact
        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithPager(PageFilter filters)
        {
            var result = await _productService.SearchType(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetProductType()
        {
  
            var result = GetListWithPager(null);
            //其他代码省略
            return Json(new SelectValues() { status = true, data = new JsonNetResult(result) }, JsonRequestBehavior.AllowGet);

        }

        public class SelectValues
        {
            public bool status { set; get; }
            public string data { set; get; }
        }
        //async Task<JsonResult>  GetProductType()
        //{
        //    var result = await _productService.SearchType();
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}

        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View(new ProductTypeDto());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Add(ProductTypeDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.AddType(dto);
                if (result.IsNotBlank())
                    return RedirectToAction("Index");
            }
            return View(dto);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            var dto = await _productService.FindType(id);
            return View(dto);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductTypeDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.UpdateType(dto);
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
                result.flag = await _productService.Delete(ids);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
 
    }
}