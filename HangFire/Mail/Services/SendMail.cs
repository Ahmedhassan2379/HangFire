using HangFire.Mail.Interfaces;
using MimeKit;
using HangFire.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using HangFire.Dtos.Mails;
using System.Net.Mail;
using HangFire.ForgetPasswordVerifcation.EmailModel;
using System.Net;
using HangFire.Helpers;

namespace HangFire.Mail.Services
{
    public class SendMail : ISendMails
    {
        private readonly MailSetting _mailSetting;
        private readonly IConfiguration _config;
        public SendMail(IOptions<MailSetting> mailsetting,IConfiguration config)
        {
            _mailSetting = mailsetting.Value;
            _config = config;
        }


        public async Task SendEmailWithAttachment(string recipient, string subject, string body, byte[] attachmentBytes=null, string attachmentFileName=null)
        {
            MailMessage mail = new MailMessage();
            mail.From = (MailAddress?)MailboxAddress.Parse(_mailSetting.Email);
            mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.AlternateViews.Add(
            AlternateView.CreateAlternateViewFromString(
                mail.Body,
                new System.Net.Mime.ContentType("text/html")
            )
        );

            // Create an attachment from the byte array
            //Attachment attachment = new Attachment(new MemoryStream(attachmentBytes), attachmentFileName);
            //mail.Attachments.Add(attachment);

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            smtpClient.Host = _mailSetting.Host;
            smtpClient.Port = _mailSetting.Port;
            smtpClient.Credentials = new System.Net.NetworkCredential(_mailSetting.Email, _mailSetting.PassWord);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}
