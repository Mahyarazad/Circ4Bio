using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Domain.NotificationAggregate;

namespace AM.Application
{
    public class NotificationApplicaiton : INotificationApplication
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationApplicaiton(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public OperationResult MarkRead(long Id)
        {
            var result = new OperationResult();
            if (_notificationRepository.Exist(x => x.Id == Id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _notificationRepository.Get(Id);
            target.MarkRead();
            _notificationRepository.SaveChanges();
            return result.Succeeded();

        }

        public OperationResult MarkReadAll()
        {
            var result = new OperationResult();
            try
            {
                foreach (var notification in _notificationRepository.GetList())
                {
                    notification.MarkRead();
                }
                _notificationRepository.SaveChanges();
                return result.Succeeded();
            }
            catch (Exception e)
            {
                return result.Failed(e.Message);
            }
        }

        public OperationResult PushNotification(NotificationViewModel Command)
        {
            var result = new OperationResult();
            var receipents = new List<Recipient>();
            foreach (var item in Command.RecipientList)
            {
                receipents.Add(new Recipient(item.UserId, item.RoleId));
            }
            var notification = new Notification(Command.NotificationBody, Command.NotificationTitle,
                receipents, Command.SenderId, Command.UserId);
            _notificationRepository.Create(notification);
            _notificationRepository.SaveChanges();
            return result.Succeeded();
        }

        public List<NotificationViewModel> GetAll()
        {
            return _notificationRepository.GetList()
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    SenderId = x.SenderId,
                    NotificationBody = x.NotificationBody,
                    NotificationTitle = x.NotificationTitle,
                    RecipientList = _notificationRepository.GetRecipientViewModel(x.Id)
                }).OrderByDescending(x => x.Id).ToList();
        }
    }
}