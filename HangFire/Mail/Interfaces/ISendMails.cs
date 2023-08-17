using HangFire.Dtos.Mails;

namespace HangFire.Mail.Interfaces
{
    public interface ISendMails
    {
        //Task SendMailAsync(MailRequestDto mailDto);
         Task SendEmailWithAttachment(string recipient, string subject, string body, byte[] attachmentBytes, string attachmentFileName);
    }
}
