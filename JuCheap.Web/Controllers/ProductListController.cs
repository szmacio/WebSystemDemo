﻿using AutoMapper;
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

namespace JuCheap.Web.Controllers
{
    public class ProductListController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        // GET: SinglePageculturecontact
        public ProductListController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [IgnoreRightFilter]
        public async Task<JsonResult> GetListWithPager(PageFilter filters)
        {
            var result = await _productService.Search(filters);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

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
            return View(new ProductDto());
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> Add(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                string content = Request.Form["editorValue"];
                dto.Procontent= Server.HtmlEncode(content);
                var result = await _productService.Add(dto);
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
            var dto = await _productService.Find(id);
            return View(dto);
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                var result = await _productService.Update(dto);
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