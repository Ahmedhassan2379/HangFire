using HangFire.Mail.Interfaces;
using MimeKit;
using HangFire.Settings;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using HangFire.Dtos.Mails;
using System.Net.Mail;
using HangFire.ForgetPasswordVerifcation.EmailModel;

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

        public void sendEmail(EmailModel emailModel)
        {
            var email = new MailMessage();
            var from =(MailAddress?)MailboxAddress.Parse(_mailSetting.Email);
            email.To.Add((MailAddress)new MailboxAddress(emailModel.To, emailModel.To));
            email.Subject = emailModel.Subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = String.Format(emailModel.Content)
            }.ToString();
            using(var client = new System.Net.Mail.SmtpClient())
            {
                try
                {
                    //client.Connect(_config["MailSettings.Host"], 587,true);
                    //client.Authenticate(_config["MailSettings.Email"], _config["MailSettings.Password"]);
                    //client.Send(email);
                    client.Host = _mailSetting.Host;
                    client.Port = _mailSetting.Port;
                    client.Credentials = new System.Net.NetworkCredential(_mailSetting.Email, _mailSetting.PassWord);
                    client.EnableSsl = true;
                    client.Send(email);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

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
