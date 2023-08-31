namespace HangFire.ForgetPasswordVerifcation.UtilityService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configure;
        public EmailService(IConfiguration configure)
        {
            _configure = configure;
        }

        public void sendEmail(EmailModel.EmailModel emailModel)
        {
            throw new NotImplementedException();
        }
    }
}
