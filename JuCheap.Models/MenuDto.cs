using System;
using System.ComponentModel.DataAnnotations;
using JuCheap.Infrastructure.Attributes;
using JuCheap.Infrastructure.Extentions;
using JuCheap.Models.Enum;

namespace JuCheap.Models
{
    /// <summary>
    /// 菜单模型
    /// </summary>
    public class MenuDto
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [ExportProperty("菜单编号", 0)]
        public string Id { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Display(Name = "上级菜单")]
        public string ParentId { get; set; }

        /// <summary>
        /// 上级菜单名称
        /// </summary>
        [Display(Name = "上级菜单")]
        public string ParentName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Display(Name = "菜单名称")]
        [Required(ErrorMessage = Message.Required)]
        [MinLength(2, ErrorMessage = Message.MinLength)]
        [MaxLength(20, ErrorMessage = Message.MaxLength)]
        [ExportProperty("菜单名称", 1)]
        public string Name { get; set; }

        /// <summary>
        /// URL
        /// </summary>
        [Display(Name = "菜单网址")]
        [Required(ErrorMessage = Message.Required)]
        [MaxLength(300, ErrorMessage = Message.MaxLength)]
        [ExportProperty("菜单网址", 2)]
        public string Url { get; set; }

        /// <summary>
        /// 排序越大越靠后
        /// </summary>
        [Display(Name = "排序数字")]
        public int Order { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Display(Name = "图标")]
        public string Icon { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        public MenuType Type { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 菜单类型名称
        /// </summary>
        public string TypeName
        {
            get { return Type.ToDescription(); }
        }
    }
}
