using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Notification;

namespace AM.Domain.NotificationAggregate
{
    public interface INotificationRepository : IRepository<long, Notification>
    {
        List<RecipientViewModel> GetRecipientViewModel(long Id);
        int CountUnRead(long Id);
    }
}