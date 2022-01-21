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
        private readonly INotificationRepository _notificationRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        public NotificationApplicaiton(INotificationRepository notificationRepository,
            IHttpContextAccessor contextAccessor)
        {
            _notificationRepository = notificationRepository;
            _contextAccessor = contextAccessor;
        }

        public OperationResult MarkRead(long Id)
        {
            var result = new OperationResult();
            if (!_notificationRepository.Exist(x => x.Id == Id) || Id == 0)
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

        public List<NotificationViewModel> GetAll(long Id)
        {
            return _notificationRepository.GetList()
                .Where(x => x.UserId == Id)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    SenderId = x.SenderId,
                    NotificationBody = x.NotificationBody,
                    NotificationTitle = x.NotificationTitle,
                    IsReed = x.IsReed,
                    RecipientList = _notificationRepository.GetRecipientViewModel(x.Id)
                }).OrderByDescending(x => x.Id).ToList();
        }

        public List<NotificationViewModel> GetAllUnread(long Id)
        {
            return _notificationRepository.GetList()
                .Where(x => x.UserId == Id && !x.IsReed)
                .Select(x => new NotificationViewModel
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    SenderId = x.SenderId,
                    NotificationBody = x.NotificationBody,
                    NotificationTitle = x.NotificationTitle,
                    IsReed = x.IsReed,
                    RecipientList = _notificationRepository.GetRecipientViewModel(x.Id)
                }).OrderByDescending(x => x.Id).ToList();
        }

        public int CountUnread(long Id)
        {
            return _notificationRepository.CountUnRead(Id);
        }
    }
}