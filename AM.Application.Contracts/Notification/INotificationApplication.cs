using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Notification
{
    public interface INotificationApplication
    {
        Task<OperationResult> MarkRead(long Id);
        Task<long> PushNotification(NotificationViewModel Command);
        Task<List<NotificationViewModel>> GetAllUnread(long Id);
        Task<OperationResult> MarkAllRead(long Id);
        Task<List<NotificationViewModel>> GetLastNUnread(long Id, int nNumber);
        Task<int> CountUnread(long Id);

    }
}