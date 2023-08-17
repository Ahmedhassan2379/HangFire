using HangFire.Mail.Interfaces;
using MimeKit;
using HangFire.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using HangFire.Dtos.Mails;
using System.Net.Mail;

namespace HangFire.Mail.Services
{
    public class SendMail : ISendMails
    {
        private readonly MailSetting _mailSetting;
        public SendMail(IOptions<MailSetting> mailsetting)
        {
            _mailSetting = mailsetting.Value;
        }
        //public async Task SendMailAsync(MailRequestDto mailDto)
        //{


        //    var email = new MimeMessage()
        //    {
        //        Sender = MailboxAddress.Parse(_mailSetting.Email),
        //        Subject = mailDto.Subject,
        //    };
        //    email.To.Add(MailboxAddress.Parse(mailDto.ToEmail));
        //    var builder = new BodyBuilder();
        //    if (mailDto.Attachments != null)
        //    {
        //        byte[] filebytes;
        //        foreach (var file in mailDto.Attachments)
        //        {
        //            if (file.Length > 0)
        //            {
        //                using var mss = new MemoryStream();
        //                file.CopyTo(mss);
        //                filebytes=mss.ToArray();
        //                builder.Attachments.Add(file.FileName, filebytes, ContentType.Parse(file.ContentType));
        //            }
        //        }
        //    }
        //    builder.HtmlBody = mailDto.Body;
        //    email.Body = builder.ToMessageBody();
        //    email.From.Add(new MailboxAddress(_mailSetting.DisplayName,_mailSetting.Email));
        //    using var smtp = new SmtpClient();
        //    smtp.Timeout = 5000;
        //    smtp.Connect(_mailSetting.Host, _mailSetting.Port,SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_mailSetting.Email, _mailSetting.PassWord);
        //    await smtp.SendAsync(email);
        //    smtp.Disconnect(true);

        //}

        public async Task SendEmailWithAttachment(string recipient, string subject, string body, byte[] attachmentBytes, string attachmentFileName)
        {
            MailMessage mail = new MailMessage();
            mail.From = (MailAddress?)MailboxAddress.Parse(_mailSetting.Email);
            mail.To.Add(recipient);
            mail.Subject = subject;
            mail.Body = body;

            // Create an attachment from the byte array
            Attachment attachment = new Attachment(new MemoryStream(attachmentBytes), attachmentFileName);
            mail.Attachments.Add(attachment);

            System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient();
            smtpClient.Host = _mailSetting.Host;
            smtpClient.Port = _mailSetting.Port;
            smtpClient.Credentials = new System.Net.NetworkCredential(_mailSetting.Email, _mailSetting.PassWord);
            smtpClient.EnableSsl = true;

            smtpClient.Send(mail);
        }
    }
}
