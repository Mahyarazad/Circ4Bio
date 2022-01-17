using System;
using System.Collections.Generic;
using System.Linq;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.ViewComponents
{
    public class NotificationViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationApplication _notificationApplication;
        public List<NotificationViewModel> Command;

        public NotificationViewComponent(IHttpContextAccessor contextAccessor, INotificationApplication notificationApplication)
        {
            _contextAccessor = contextAccessor;
            _notificationApplication = notificationApplication;
        }

        public IViewComponentResult Invoke()
        {
            Command = _notificationApplication.GetAll(
                long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value));
            return View(Command);
        }
        public IViewComponentResult OnPostMarkRead(long Id)
        {
            var result = _notificationApplication.MarkRead(Id);
            return View();
        }
    }
}