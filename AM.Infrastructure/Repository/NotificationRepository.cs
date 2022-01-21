using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Notification;
using AM.Domain.NotificationAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class NotificationRepository : RepositoryBase<long, Notification>, INotificationRepository
    {
        private readonly AMContext _amContext;
        public NotificationRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<RecipientViewModel> GetRecipientViewModel(long Id)
        {
            var recipientList = new List<RecipientViewModel>();
            var query = _amContext.Notifications.AsNoTracking()
                .FirstOrDefault(x => x.Id == Id).Recipient;
            foreach (var item in query)
            {
                recipientList.Add(new RecipientViewModel
                {
                    RoleId = item.RoleId,
                    UserId = item.UserId
                });
            }

            return recipientList;
        }

        public int CountUnRead(long Id)
        {
            return _amContext.Notifications
                .AsNoTracking()
                .Where(x => !x.IsReed && x.UserId == Id)
                .Select(x => x.UserId == Id).ToList().Count;
        }
    }
}