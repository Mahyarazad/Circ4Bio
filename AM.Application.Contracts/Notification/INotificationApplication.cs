using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Notification
{
    public interface INotificationApplication
    {
        OperationResult MarkRead(long Id);
        long PushNotification(NotificationViewModel Command);
        List<NotificationViewModel> GetAllUnread(long Id);
        List<NotificationViewModel> GetLastNUnread(long Id, int nNumber);
        int CountUnread(long Id);

    }
}