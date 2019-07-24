using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Vsan.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class EmailHelper
    {
        // SendMailb("496988878@qq.com", SystemInformation.ComputerName, "iaglvtoiuqfubiee", "786198495@qq.com", "客户端启动了时间:" + DateTime.Now.ToString("MM月dd日 HH:mm:ss"), "测试账号：" + user.useName + "  \n测试密码：" + user.pwd + "  异常: " + exs, "smtp.qq.com", true);
        public static void SendMailb(string mailSender, string displayName, string mailPwd, string sendTo, string subject, string messageBody, string smtpHost, bool isBodyHtml)
        {
            SendMailb(mailSender, displayName, mailPwd, new string[] { sendTo }, subject, messageBody, smtpHost, isBodyHtml);
        }
        public static void SendMailb(string mailSender, string displayName, string mailPwd, string[] sendToArr, string subject, string messageBody, string smtpHost, bool isBodyHtml)
        {
            try
            {
                SmtpClient smtp = new SmtpClient();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = true;
                smtp.Host = smtpHost;
                smtp.Port = 25;
                smtp.UseDefaultCredentials = true;
                //smtp.EnableSsl?=?true;
                smtp.Credentials = new NetworkCredential(mailSender, mailPwd);
                MailMessage mm = new MailMessage();
                //实例化一个邮件类
                mm.Priority = MailPriority.High;
                mm.From = new MailAddress(mailSender, displayName, Encoding.GetEncoding(936));

                mm.Sender = new MailAddress(mailSender, displayName, Encoding.GetEncoding(936));

                if (sendToArr != null && sendToArr.Length > 0)
                {
                    for (int i = 0; i < sendToArr.Length; i++)
                    {
                        mm.To.Add(sendToArr[i]);

                    }
                }
                //System.Net.Mail.Attachment myAttachment = new System.Net.Mail.Attachment("C://Kugou/Kugou/Kugou/" + b + ".xml", System.Net.Mime.MediaTypeNames.Application.Octet);
                //MIME协议下的一个对象，用以设置附件的创建时间，修改时间以及读取时间
                // ContentDisposition disposition = myAttachment.ContentDisposition;
                //disposition.CreationDate = System.IO.File.GetCreationTime(file);
                //disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
                //disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
                //用smtpclient对象里attachments属性，添加上面设置好的myattachment
                // mm.Attachments.Add(myAttachment);
                mm.Subject = subject;
                //邮件标题
                mm.SubjectEncoding = Encoding.GetEncoding(936);
                mm.IsBodyHtml = isBodyHtml;
                //邮件正文是否是HTML格式
                mm.BodyEncoding = Encoding.GetEncoding(936);
                //邮件正文的编码，?设置不正确，?接收者会收到乱码
                mm.Body = messageBody;
                //邮件正文
                smtp.Send(mm);
                //发送邮件，如果不返回异常，?则大功告成了。
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
