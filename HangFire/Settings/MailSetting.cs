using System.Net.Mail;

namespace HangFire.Settings
{
    public class MailSetting
    {
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string DisplayName { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }

    }
}
