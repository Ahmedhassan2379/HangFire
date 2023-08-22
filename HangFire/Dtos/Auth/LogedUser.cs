namespace HangFire.Dtos.Auth
{
    public class LogedUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? Token { get; set; } = "";
        public string? Role { get; set; } = "user";
    }
}
