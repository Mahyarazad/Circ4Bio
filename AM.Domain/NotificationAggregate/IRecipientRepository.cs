using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Notification;

namespace AM.Domain.NotificationAggregate
{
    public interface IRecipientRepository : IRepository<long, Recipient>
    {
        Task<List<RecipientViewModel>> GetAllUnread(long Id);
        Task<OperationResult> MarkRead(long Id);
    }
}