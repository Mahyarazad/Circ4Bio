namespace AM.Domain.RoleAggregate
{
    public class Permission
    {
        public Permission(int code)
        {
            Code = code;
        }

        public long Id { get; private set; }
        public int Code { get; private set; }
        public string? Name { get; private set; }
        public int RoleId { get; private set; }
        public Role? Role { get; private set; }
    }
}