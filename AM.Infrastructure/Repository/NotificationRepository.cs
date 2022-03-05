using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Notification;
using AM.Domain.NegotiateAggregate;
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

        public Task<List<RecipientViewModel>> GetRecipientViewModel(long Id)
        {
            var recipientList = new List<RecipientViewModel>();
            var query = _amContext.Notifications.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == Id).Result.Recipient;
            foreach (var item in query)
            {
                recipientList.Add(new RecipientViewModel
                {
                    RoleId = item.RoleId,
                    UserId = item.UserId
                });
            }

            return Task.FromResult(recipientList);
        }

        public Task<OperationResult> MarkAllRead(long Id)
        {
            foreach (var item in _amContext.Recipients.Where(x => x.UserId == Id).ToList())
            {
                item.MarkRead();
            }
            _amContext.SaveChanges();
            return Task.FromResult(new OperationResult().Succeeded());
        }

        public async Task<List<NotificationViewModel>> GetLastNUnread(long Id, int nNumber)
        {
            List<NotificationViewModel> result =
                 await _amContext.Recipients.AsNoTracking()
                .Where(x => x.UserId == Id && !x.IsReed)
                .Include(x => x.Notification)
                .Select(x => new NotificationViewModel
                {
                    UserId = x.UserId,
                    Id = x.NotificationId,
                    RecipientId = x.Id,
                    RedirectUrl = x.Notification.RedirectUrl,
                    NotificationBody = x.Notification.NotificationBody,
                    NotificationTitle = x.Notification.NotificationTitle,
                }).OrderByDescending(x => x.Id)
                .Take(nNumber)
                .ToListAsync();
            return result;
        }

        public async Task<List<NotificationViewModel>> GetAllUnread(long Id)
        {
            List<NotificationViewModel> result = await _amContext.Recipients
                .AsNoTracking()
                .Include(x => x.Notification)
                .Where(x => x.UserId == Id && !x.IsReed)
                .Select(x => new NotificationViewModel
                {
                    UserId = x.UserId,
                    Id = x.NotificationId,
                    RecipientId = x.Id,
                    RedirectUrl = x.Notification.RedirectUrl,
                    NotificationBody = x.Notification.NotificationBody,
                    NotificationTitle = x.Notification.NotificationTitle,
                    CreationTime = x.CreationTime
                })
                .OrderByDescending(z => z.Id)
                .ToListAsync();
            return result;
        }

        public Task<int> CountUnRead(long Id)
        {
            return Task.FromResult(_amContext.Recipients
                .AsNoTracking()
                .Where(x => x.UserId == Id && !x.IsReed).ToList().Count);
        }
    }
}