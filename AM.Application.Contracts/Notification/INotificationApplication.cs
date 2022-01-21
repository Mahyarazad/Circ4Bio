using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Notification
{
    public interface INotificationApplication
    {
        OperationResult MarkRead(long Id);
        OperationResult MarkReadAll();
        OperationResult PushNotification(NotificationViewModel Command);
        List<NotificationViewModel> GetAll(long Id);
        List<NotificationViewModel> GetAllUnread(long Id);
        int CountUnread(long Id);

    }
}