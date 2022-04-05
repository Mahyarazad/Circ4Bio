namespace AM.Application.Contracts.User
{
    public class UserViewModel
    {
        public long Id { get; set; }
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Avatar { get; set; }
        public string? Role { get; set; }
        public int RoleId { get; set; }
        public string? CreationTime { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
    }
}