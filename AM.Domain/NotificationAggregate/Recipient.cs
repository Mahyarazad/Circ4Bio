namespace AM.Domain.NotificationAggregate
{
    public class Recipient
    {
        public Recipient(long userId, long roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }

        public long Id { get; private set; }
        public long UserId { get; private set; }
        public long RoleId { get; private set; }
    }
}