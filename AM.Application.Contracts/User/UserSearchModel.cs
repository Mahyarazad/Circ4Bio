namespace AM.Application.Contracts.User
{
    public class UserSearchModel
    {
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public long Role { get; set; }

    }
}