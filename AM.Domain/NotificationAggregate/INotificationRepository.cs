using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Notification;

namespace AM.Domain.NotificationAggregate
{
    public interface INotificationRepository : IRepository<long, Notification>
    {
        Task<List<RecipientViewModel>> GetRecipientViewModel(long Id);
        Task<List<NotificationViewModel>> GetAllUnread(long Id);
        Task<OperationResult> MarkAllRead(long Id);
        Task<List<NotificationViewModel>> GetLastNUnread(long Id, int nNumber);
        Task<int> CountUnRead(long Id);
    }
}