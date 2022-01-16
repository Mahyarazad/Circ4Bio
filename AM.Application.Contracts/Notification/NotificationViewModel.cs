using System.Collections.Generic;

namespace AM.Application.Contracts.Notification
{
    public class NotificationViewModel
    {
        public long Id { get; set; }
        public string? NotificationBody { get; set; }
        public string? NotificationTitle { get; set; }
        public long SenderId { get; set; }
        public long UserId { get; set; }
        public List<RecipientViewModel>? RecipientList { get; set; }
    }
    public class RecipientViewModel
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
    }
}