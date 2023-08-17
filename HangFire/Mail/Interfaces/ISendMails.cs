namespace HangFire.Mail.Interfaces
{
    public interface ISendMails
    {
        Task SendMailAsync(string MailTo, string Subject, string Body,IList<IFormFile> attachement=null);
    }
}
