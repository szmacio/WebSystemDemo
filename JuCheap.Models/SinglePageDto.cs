using System;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{


    /// <summary>
    /// 登录日志
    /// </summary>
    public class SinglePageDto
    {

        /// <summary>
        /// 页面类型
        /// </summary>
        [Display(Name = "页面名称")]
        public string PageName { get; set; }

        /// <summary>
        /// 页面标题
        /// </summary>
        [Display(Name = "标题")]
        public string Title { get; set; }

        /// <summary>
        /// 页面内容
        /// </summary>
      [Display(Name = "内容")]
        public string Content { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [Display(Name = "作者")]
        public string Author { get; set; }
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
