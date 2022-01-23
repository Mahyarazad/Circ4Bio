using _0_Framework.Domain;

namespace AM.Domain.NotificationAggregate
{
    public class Recipient : BaseEntity<long>
    {
        public Recipient(long userId, int roleId, long notificationId)
        {
            UserId = userId;
            RoleId = roleId;
            IsReed = false;
            NotificationId = notificationId;
        }
        public void MarkRead()
        {
            IsReed = true;
        }

        public long UserId { get; private set; }
        public int RoleId { get; private set; }
        public bool IsReed { get; private set; }
        public long NotificationId { get; set; }
        public Notification? Notification { get; set; }
    }
}