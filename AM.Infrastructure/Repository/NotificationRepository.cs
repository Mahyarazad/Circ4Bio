using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using _0_Framework.Application;
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

        public List<NotificationViewModel> GetLast5Unread(long Id)
        {
            return _amContext.Recipients
                .AsNoTracking()
                .Where(x => x.UserId == Id && !x.IsReed)
                .Include(x => x.Notification)
                .Select(x => new NotificationViewModel
                {
                    UserId = x.UserId,
                    Id = x.NotificationId,
                    RecipientId = x.Id,
                    NotificationBody = x.Notification.NotificationBody,
                    NotificationTitle = x.Notification.NotificationTitle,
                }).OrderByDescending(x => x.Id)
                .Reverse().Take(7).Reverse()
                .ToList();
        }

        public List<NotificationViewModel> GetAllUnread(long Id)
        {
            return _amContext.Recipients
                .AsNoTracking()
                .Where(x => x.UserId == Id && !x.IsReed)
                .Include(x => x.Notification)
                .Select(x => new NotificationViewModel
                {
                    UserId = x.UserId,
                    Id = x.NotificationId,
                    RecipientId = x.Id,
                    NotificationBody = x.Notification.NotificationBody,
                    NotificationTitle = x.Notification.NotificationTitle,
                }).OrderByDescending(x => x.Id)
                .Reverse().Take(7).Reverse()
                .ToList();
        }

        public int CountUnRead(long Id)
        {
            return _amContext.Recipients
                .AsNoTracking()
                .Where(x => x.UserId == Id && !x.IsReed).ToList().Count;
        }
    }
}