﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JuCheap.Infrastructure.Utilities
{
   public static class Pagedlist
    {
        #region 属性Property
        /// <summary>
        /// 当前页码
        /// </summary>
        private static int? _currentPage = null;
        /// <summary>
        /// 当前页码
        /// </summary>
        public static int CurrentPage
        {
            get
            {
                return _currentPage ?? 1;
            }
            set
            {
                _currentPage = value;
            }
        }
        /// <summary>
        /// 每页记录条数
        /// </summary>
        private static int? _pageSize = null;
        /// <summary>
        /// 每页记录条数
        /// </summary>
        public static int PageSize
        {
            get
            {
                return _pageSize ?? 15;
            }
            set
            {
                _pageSize = value;
            }
        }
        /// <summary>
        /// 是否显示上一页
        /// </summary>
        public static bool HasPreviousPage
        {
            get
            {
                return (CurrentPage > 1);
            }
        }
        /// <summary>
        /// 是否显示下一页
        /// </summary>
        public static bool HasNextPage
        {
            get
            {
                return (CurrentPage < TotalPages);
            }
        }
        /// <summary>
        /// 当前页:
        /// </summary>
        public static string CurrentPageDisplayName { get; set; }
        /// <summary>
        /// 每页显示:
        /// </summary>
        public static string PageSizeDisplayName { get; set; }
        public static string FirstDisplayName { get; set; }
        public static string PreDisplayName { get; set; }
        public static string NextDisplayName { get; set; }
        public static string LastDisplayName { get; set; }
        public static string TotalCountDisplayName { get; set; }
        public static string TotalPagesDisplayName { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        public static int TotalCount
        {
            get;
            set;
        }
        public static int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(TotalCount / (double)PageSize);
                //return (TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1);
            }
        }
        /// <summary>
        /// 设置分页url eg:/Admin/Product/Index
        /// </summary>
        public static string PagingUrl
        {
            get;
            set;
        }
        /// <summary>
        /// 默认page,设置分页参数名 eg:/Admin/Product/Index?PagingParamName=1
        /// </summary>
        public static string PagingParamName
        {
            get;
            set;
        }
        #endregion
        #region Paging String
        /// <summary>
        /// MVC分页 如果用jquery分页只需要class不需要href,用以下实现:
        /// $(".class值").live("click", function () {
        /// var page = $(this).attr("pagingParamName值");
        /// $("#order").html("").load("/Customer/Order?page="+page);
        /// });live自动给遍历增加事件
        /// </summary>
        /// <param name="html"></param>
        /// <param name="htmlAttributes">new {@class="grey",pagingParamName="page",href="/Admin/Product/Index" rel="external nofollow" } pagingParamName默认page,匿名类添加控件属性</param>
        /// <returns></returns>
        public static MvcHtmlString Paging(this System.Web.Mvc.HtmlHelper html, object htmlAttributes)
        {
            RouteValueDictionary values = new RouteValueDictionary(htmlAttributes);
            #region 属性赋值
            if (values["href"] != null)
            {
                PagingUrl = values["href"].ToString();
            }
            if (values["pagingParamName"] != null)
            {
                PagingParamName = values["pagingParamName"].ToString();
                values.Remove("pagingParamName");
            }
            else
            {
                PagingParamName = "page";
            }
            #endregion
            #region 分页最外层div/span
            TagBuilder builder = new TagBuilder("div");//span
                                                       //创建Id,注意要先设置IdAttributeDotReplacement属性后再执行GenerateId方法. 
                                                       //builder.IdAttributeDotReplacement = "_";
                                                       //builder.GenerateId(id);
                                                       //builder.AddCssClass("");
                                                       //builder.MergeAttributes(values);
            builder.InnerHtml = PagingBuilder(values);
            #endregion
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.Normal));//解决直接显示html标记
        }
        private static string PagingBuilder(RouteValueDictionary values)
        {
            #region 条件搜索时包括其他参数
            StringBuilder urlParameter = new StringBuilder();
            NameValueCollection collection = HttpContext.Current.Request.QueryString;
            string[] keys = collection.AllKeys;
            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i].ToLower() != "page")
                {
                    urlParameter.AppendFormat("&{0}={1}", keys[i], collection[keys[i]]);
                }
            }
            #endregion
            //CurrentPage = Convert.ToInt32(HttpContext.Current.Request.QueryString["page"] ?? "0");
            StringBuilder sb = new StringBuilder();
            #region 分页统计
            sb.AppendFormat("Total  {0}   Records Page  {1} of  {2}   ", TotalCount, CurrentPage, TotalPages);
            #endregion
            #region 首页 上一页
            sb.AppendFormat(TagBuilder(values, 1, " First"));
            //sb.AppendFormat("<a href={0}?page=1{1}>First</a> ",url,urlParameter);
            if (HasPreviousPage)
            {
                sb.AppendFormat(TagBuilder(values, CurrentPage - 1, " Prev "));
                //sb.AppendFormat("<a href={0}?page={1}{2}>Prev</a> ", url, CurrentPage - 1, urlParameter);
            }
            #endregion
            #region 分页逻辑
            if (TotalPages > 10)
            {
                if ((CurrentPage + 5) < TotalPages)
                {
                    if (CurrentPage > 5)
                    {
                        for (int i = CurrentPage - 5; i <= CurrentPage + 5; i++)
                        {
                            sb.Append(TagBuilder(values, i, i.ToString()));
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            sb.Append(TagBuilder(values, i, i.ToString()));
                        }
                    }
                    sb.Append("... ");
                }
                else
                {
                    for (int i = CurrentPage - 10; i <= TotalPages; i++)
                    {
                        sb.Append(TagBuilder(values, i, i.ToString()));
                    }
                }
            }
            else
            {
                for (int i = 1; i <= TotalPages; i++)
                {
                    sb.Append(" " + TagBuilder(values, i, i.ToString()) + " ");
                }
            }
            #endregion
            #region 下一页 末页
            if (HasNextPage)
            {
                sb.AppendFormat(TagBuilder(values, CurrentPage + 1, "Next"));
                //sb.AppendFormat("<a href={0}?page={1}{2}>Next</a> ", url, CurrentPage + 1, urlParameter);
            }
            sb.AppendFormat(TagBuilder(values, TotalPages, "Last"));
            //sb.AppendFormat("<a href={0}?page={1}{2}>Last</a>",url,TotalPages,urlParameter);
            #endregion
            return sb.ToString();
        }
        private static string TagBuilder(RouteValueDictionary values, int i, string innerText)
        {
            values[PagingParamName] = i;
            TagBuilder tag = new TagBuilder("a");
            if (PagingUrl != null)
            {
                values["href"] = PagingUrl + "?" + PagingParamName + "= " + i + "   ";
            }
            if (CurrentPage == i && innerText != " First" && innerText != " Last")
            {
                values["id"] = "on";
            }
            else
            {
                tag.Attributes["id"] = "";
            }
            tag.MergeAttributes(values);
            tag.SetInnerText(innerText);
            return tag.ToString();
        }
        #endregion

    }
}
