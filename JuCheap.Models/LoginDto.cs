using System.ComponentModel.DataAnnotations;

namespace JuCheap.Models
{
    /// <summary>
    /// 登录DTO
    /// </summary>
    public class LoginDto
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Display(Name = "登录账号")]
        [Required(ErrorMessage = Message.Required)]
        [MinLength(4, ErrorMessage = Message.MinLength)]
        [MaxLength(20, ErrorMessage = Message.MaxLength)]
        [RegularExpression("^[^_][a-zA-Z0-9_]*$", ErrorMessage = "登录账号必须是字母、数字或者下划线的组合")]
        public string LoginName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "登录密码")]
        [Required(ErrorMessage = Message.Required)]
        [MinLength(6,ErrorMessage = Message.MinLength)]
        [MaxLength(12, ErrorMessage = Message.MaxLength)]
        public string Password { get; set; }

        /// <summary>
        /// 记住我
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// 登陆成功后跳转的地址
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 登录IP
        /// </summary>
        public string LoginIP { get; set; }
    }
}
