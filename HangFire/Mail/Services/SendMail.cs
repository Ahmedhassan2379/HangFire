using HangFire.Mail.Interfaces;
using MimeKit;
using HangFire.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace HangFire.Mail.Services
{
    public class SendMail : ISendMails
    {
        private readonly MailSetting _mailSetting;
        public SendMail(IOptions<MailSetting> mailsetting)
        {
            _mailSetting = mailsetting.Value;
        }
        public async Task SendMailAsync(string MailTo, string Subject, string Body, IList<IFormFile> attachement = null)
        {

          
            var email = new MimeMessage()
            {
                Sender = MailboxAddress.Parse(_mailSetting.Email),
                Subject = Subject
            };
            email.To.Add(MailboxAddress.Parse(MailTo));
            var builder = new BodyBuilder();
            if (attachement != null)
            {
                byte[] filebytes;
                foreach (var file in attachement)
                {
                    if (file.Length > 0)
                    {
                        using var mss = new MemoryStream();
                        file.CopyTo(mss);
                        filebytes=mss.ToArray();
                        builder.Attachments.Add(file.FileName, filebytes, ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = Body;
            email.Body = builder.ToMessageBody();
            email.From.Add(new MailboxAddress(_mailSetting.DisplayName,_mailSetting.Email));
            using var smtp = new SmtpClient();
            smtp.Timeout = 5000;
            smtp.Connect(_mailSetting.Host, _mailSetting.Port,SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSetting.Email, _mailSetting.PassWord);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
