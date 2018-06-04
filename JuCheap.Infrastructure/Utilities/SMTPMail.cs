/* ========================================================		
* Module Name: JuCheap.Core.Email
* Class  Name: FTMailBase
* Description: 所有邮件Sender的基类
* Company    : www.jucheap.com
* Author     : dj.wong
* Create Date: 2014-11-03
===========================================================*/

using System;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using JuCheap.Infrastructure.Extentions;

namespace JuCheap.Infrastructure.Utilities
{
    internal class SmtpMailer : MailBase
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(typeof(SmtpMailer));

        /// <summary>
        /// SMTP邮件Sender的类的封装
        /// </summary>
        public override async Task<bool> Send()
        {
            var isSended = false;
            try
            {
                //qq域名邮箱的smtp(smtp.qq.com)，端口号：25；如果是腾讯企业邮箱，需要改成smtp.exmail.qq.com，端口号465
                var smtpServer = "smtp.exmail.qq.com";
                var userName = "123456@qq.com";//用户名
                var password = "123456";//密码
                //注意：如果是腾讯企业邮箱发送邮件，默认端口是465，不需要设置，如果你设置了，就发不出来邮件，不知道原因
                var message = new MailMessage();
                To.ForEach(email =>
                {
                    if (email.IsEmail())
                        message.To.Add(email);
                });
                message.Sender = new MailAddress(userName, userName);
                if (Cc.AnyOne())
                {
                    Cc.ForEach(email =>
                    {
                        if (email.IsEmail())
                            message.CC.Add(email);
                    });
                }
                message.Subject = Encoding.Default.GetString(Encoding.Default.GetBytes(Subject));
                if (From.IsBlank())
                {
                    From = userName;
                }
                message.From = DisplayName.IsBlank() ? new MailAddress(From) : new MailAddress(From, Encoding.Default.GetString(Encoding.Default.GetBytes(DisplayName)));

                message.Body = Encoding.Default.GetString(Encoding.Default.GetBytes(Body));
                message.BodyEncoding = Encoding.GetEncoding("GBK");
                message.HeadersEncoding = Encoding.UTF8;
                message.SubjectEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                message.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                message.ReplyToList.Add(new MailAddress(From));
                if (Attachments.AnyOne())
                {
                    Attachments.ForEach(x =>
                    {
                        message.Attachments.Add(new Attachment(x));
                    });
                }

                var smtp = new SmtpClient(smtpServer)
                {
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(userName, password),
                    Timeout = 30000,
                    EnableSsl = true
                };

                smtp.SendCompleted += smtp_SendCompleted;
                await smtp.SendMailAsync(message);
                isSended = true;
            }
            catch (Exception ex)
            {
                Log.Error("发送邮件失败", ex);
            }
            return isSended;
        }
        /// <summary>
        /// 发送完成后，如果有错误，记录日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void smtp_SendCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                Log.Error("邮件发送，记录错误", e.Error);
            }
        }
    }
}