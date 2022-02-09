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
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly INotificationApplication _notificationApplication;

        public NotificationController(INotificationApplication notificationApplication,
            IAutenticateHelper autenticateHelper)
        {
            _autenticateHelper = autenticateHelper;
            _notificationApplication = notificationApplication;
        }

        [HttpPost]
        public int MarkRead(NotificationViewModel command)
        {
            if (_autenticateHelper.IsAuthenticated())
            {
                _notificationApplication.MarkRead(command.Id);
                return _notificationApplication.CountUnread(_autenticateHelper.CurrentAccountRole().Id);
            }
            else
            {
                return 0;
            }

        }

        [Route("[action]")]
        [HttpGet]
        public int CountUnreadNotification()
        {
            if (_autenticateHelper.IsAuthenticated())
            {
                return _notificationApplication.CountUnread(_autenticateHelper.CurrentAccountRole().Id);
            }

            else
            {
                return 0;
            }
        }

        [Route("[action]")]
        [HttpGet]
        public List<NotificationViewModel> Notification()
        {
            if (_autenticateHelper.IsAuthenticated())
            {
                return _notificationApplication.GetLastNUnread(_autenticateHelper.CurrentAccountRole().Id, 5);
            }
            else
            {
                return new List<NotificationViewModel>();
            }
        }
    }

}
