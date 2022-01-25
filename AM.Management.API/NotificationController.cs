using System;
using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Mvc;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationApplication _notificationApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        public NotificationController(INotificationApplication notificationApplication,
            IAutenticateHelper autenticateHelper)
        {
            _notificationApplication = notificationApplication;
            _autenticateHelper = autenticateHelper;
        }

        [HttpPost]
        public int MarkRead(NotificationViewModel command)
        {
            _notificationApplication.MarkRead(command.Id);
            return _notificationApplication.CountUnread(_autenticateHelper.CurrentAccountRole().Id);
        }
        [Route("[action]")]
        [HttpGet]
        public int CountUnreadNotification()
        {
            return _notificationApplication.CountUnread(_autenticateHelper.CurrentAccountRole().Id);
        }
        [Route("[action]")]
        [HttpGet]
        public List<NotificationViewModel> Notification()
        {
            return _notificationApplication.GetLastNUnread(_autenticateHelper.CurrentAccountRole().Id, 5);
        }
    }

}
