/* ========================================================		
* Module Name: JuCheap.Core.Email
* Class  Name: FTMailBase
* Description: 所有邮件Sender的基类
* Company    : www.jucheap.com
* Author     : dj.wong
* Create Date: 2014-11-03
===========================================================*/

using System.Collections.Generic;
using System.Threading.Tasks;

namespace JuCheap.Infrastructure.Utilities
{
    /// <summary>
    /// 所有邮件Sender的基类
    /// </summary>
    public abstract class MailBase
    {
        /// <summary>
        /// 发件人
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 收件人列表
        /// </summary>
        public List<string> To { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }
        //正文
        public string Body { get; set; }
        /// <summary>
        /// 抄送人列表
        /// </summary>
        public List<string> Cc { get; set; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 附件列表
        /// </summary>
        public IList<string> Attachments { get; set; } 

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <returns></returns>
        public abstract Task<bool> Send();

        /// <summary>
        /// 工厂方法，获取邮件SENDER实例
        /// </summary>
        public static MailBase Instance
        {
            get
            {
                return new SmtpMailer();
            }
        }
    }
}