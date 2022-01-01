namespace AM.Application.Contracts.User
{
    public class ChangePassword
    {
        public string Email { get; set; }
        public long Id { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
    }
}