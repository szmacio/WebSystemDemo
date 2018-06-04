using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{
    /// <summary>
    /// 用户DTO
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string LoginName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDateTime { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 用户拥有的角色
        /// </summary>
        public virtual IList<UserRoleDto> UserRoles { get; set; }
    }

    /// <summary>
    /// 重置密码模型
    /// </summary>
    public class ResetPasswordDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Required(ErrorMessage = "用户ID不能为空")]
        public string UserId { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Display(Name = "新密码")]
        [Required(ErrorMessage = "请输入密码")]
        [MinLength(6, ErrorMessage = "密码长度至少需要6个字符"), MaxLength(12, ErrorMessage = "密码长度最长为12个字符")]
        public string Password { get; set; }
    }
}
