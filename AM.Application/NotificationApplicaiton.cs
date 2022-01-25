using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Domain.NotificationAggregate;
using Microsoft.AspNetCore.Http;

namespace AM.Application
{
    public class NotificationApplicaiton : INotificationApplication
    {
        private readonly IRecipientRepository _recipientRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationRepository _notificationRepository;

        public NotificationApplicaiton(INotificationRepository notificationRepository,
            IRecipientRepository recipientRepository,
            IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _recipientRepository = recipientRepository;
            _notificationRepository = notificationRepository;
        }

        public OperationResult MarkRead(long Id)
        {
            var result = new OperationResult();
            if (!_recipientRepository.Exist(x => x.Id == Id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _recipientRepository.Get(Id);
            target.MarkRead();
            _recipientRepository.SaveChanges();
            return result.Succeeded();
        }

        public long PushNotification(NotificationViewModel Command)
        {
            var notification = new Notification(Command.NotificationBody, Command.NotificationTitle,
                Command.SenderId, Command.UserId);
            _notificationRepository.Create(notification);
            _notificationRepository.SaveChanges();
            return notification.Id;
        }

        public List<NotificationViewModel> GetAllUnread(long Id)
        {
            return _notificationRepository.GetAllUnread(Id);

        }

        public List<NotificationViewModel> GetLastNUnread(long Id, int nNumber)
        {
            return _notificationRepository.GetLastNUnread(Id, nNumber);
        }

        public int CountUnread(long Id)
        {
            return _notificationRepository.CountUnRead(Id);
        }
    }
}