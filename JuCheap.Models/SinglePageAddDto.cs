using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SinglePageAddDto
    {
        [Display(Name = "页面名称")]
        /// <summary>
        /// 页面类型
        /// </summary>
        public string PageName { get; set; }
        [Display(Name = "标题")]
        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title { get; set; }
        [Display(Name = "内容")]
        /// <summary>
        /// 页面内容
        /// </summary>
        public string Content { get; set; }
        [Display(Name = "作者")]
        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
