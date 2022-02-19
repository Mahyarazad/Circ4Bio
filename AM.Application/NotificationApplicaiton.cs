using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Domain.NotificationAggregate;
using Microsoft.AspNetCore.Http;

namespace AM.Application
{
    public class NotificationApplicaiton : INotificationApplication
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotificationApplicaiton(INotificationRepository notificationRepository,
            IRecipientRepository recipientRepository,
            IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _recipientRepository = recipientRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<OperationResult> MarkRead(long Id)
        {
            var result = new OperationResult();
            if (!_recipientRepository.Exist(x => x.Id == Id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = await _recipientRepository.Get(Id);
            target.MarkRead();
            _recipientRepository.SaveChanges();
            return result.Succeeded();
        }

        public Task<long> PushNotification(NotificationViewModel Command)
        {
            var notification = new Notification(Command.NotificationBody, Command.NotificationTitle,
                Command.SenderId, Command.UserId);
            _notificationRepository.Create(notification);
            _notificationRepository.SaveChanges();
            return Task.FromResult(notification.Id);
        }

        public async Task<List<NotificationViewModel>> GetAllUnread(long Id)
        {
            List<NotificationViewModel> result = await _notificationRepository.GetAllUnread(Id);
            return result;
        }

        public async Task<OperationResult> MarkAllRead(long Id)
        {
            return await _notificationRepository.MarkAllRead(Id);
        }

        public async Task<List<NotificationViewModel>> GetLastNUnread(long Id, int nNumber)
        {
            List<NotificationViewModel> result = await _notificationRepository.GetLastNUnread(Id, nNumber);
            return result;
        }

        public async Task<int> CountUnread(long Id)
        {
            return await _notificationRepository.CountUnRead(Id);
        }
    }
}