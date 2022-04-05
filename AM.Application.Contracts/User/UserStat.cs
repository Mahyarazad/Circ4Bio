namespace AM.Application.Contracts.User
{
    public class UserStat
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public string? RoleType { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
    }
}