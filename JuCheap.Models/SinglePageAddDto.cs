using System;

namespace JuCheap.Models
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class SinglePageAddDto
    {
        /// <summary>
        /// 页面类型
        /// </summary>
        public string PageName { get; set; }

        /// <summary>
        /// 页面标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 页面内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        public string Author { get; set; }
        public string Id { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
