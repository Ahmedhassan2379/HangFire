using HangFire.ForgetPasswordVerifcation.EmailModel;
namespace HangFire.ForgetPasswordVerifcation.UtilityService
{
    public interface IEmailService
    {
        void sendEmail(EmailModel.EmailModel emailModel);
    }
}
