using System.Collections.Generic;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Notification;

namespace AM.Domain.NotificationAggregate
{
    public interface IRecipientRepository : IRepository<long, Recipient>
    {
        List<RecipientViewModel> GetAllUnread(long Id);
        OperationResult MarkRead(long Id);
    }
}