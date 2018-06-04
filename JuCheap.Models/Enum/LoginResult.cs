using System.ComponentModel;

namespace JuCheap.Models.Enum
{
    /// <summary>
    /// 登陆结果
    /// </summary>
    public enum LoginResult
    {
        /// <summary>
        /// 账号不存在
        /// </summary>
        [Description("账号不存在")]
        AccountNotExists = 1,

        /// <summary>
        /// 登陆密码错误
        /// </summary>
        [Description("登陆密码错误")]
        WrongPassword =2,

        /// <summary>
        /// 登陆成功
        /// </summary>
        [Description("登陆成功")]
        Success = 3
    }
}
