using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Common
{
    public class EMailClient
    {
        public SmtpClient SmtpClient { get; protected set; }

        public EMailClient(string host, int port, string userName, string password)
        {
            this.SmtpClient = new SmtpClient()
            {
                EnableSsl = false,
                Host = host,
                Port = port,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(userName, password),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public bool SendMail(MailMessage mail)
        {
            return true;
        }

        public bool SendMail(string to, string subject, string content, bool isHtml = false, params Attachment[] attachments)
        {
            return SendMail(new string[] { to }, subject, content, isHtml, attachments);
        }

        public bool SendMail(MailAddress to, string subject, string content, bool isHtml = false, params Attachment[] attachments)
        {
            return SendMail(new MailAddress[] { to }, subject, content, isHtml, attachments);
        }

        public bool SendMail(IEnumerable<string> to, string subject, string content, bool isHtml = false, params Attachment[] attachments)
        {
            MailMessage mail = new MailMessage();
            foreach (string i in to)
            {
                mail.To.Add(i);
            }
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = isHtml;
            foreach (var i in attachments)
            {
                mail.Attachments.Add(i);
            }
            return SendMail(mail);
        }

        public bool SendMail(IEnumerable<MailAddress> to, string subject, string content, bool isHtml = false, params Attachment[] attachments)
        {
            MailMessage mail = new MailMessage();
            foreach (MailAddress i in to)
            {
                mail.To.Add(i);
            }
            mail.Subject = subject;
            mail.Body = content;
            mail.IsBodyHtml = isHtml;
            foreach (var i in attachments)
            {
                mail.Attachments.Add(i);
            }
            return SendMail(mail);
        }
    }
}
