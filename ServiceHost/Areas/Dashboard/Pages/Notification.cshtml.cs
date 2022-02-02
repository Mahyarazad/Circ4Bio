using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class NotificationModel : PageModel
    {
        public List<NotificationViewModel> Command;
        private readonly INotificationApplication _notificationApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        public NotificationModel(INotificationApplication notificationApplication,
            IHttpContextAccessor contextAccessor)
        {
            _notificationApplication = notificationApplication;
            _contextAccessor = contextAccessor;
        }

        public void OnGet(long Id)
        {
            Command = _notificationApplication.GetAllUnread(Id);
        }
        public void OnGetMarkAllRead(long Id)
        {
            _notificationApplication.MarkAllRead(Id);
            Command = _notificationApplication.GetAllUnread(Id);
        }
        public JsonResult OnPostMarkRead(long Id)
        {
            return new JsonResult(_notificationApplication.MarkRead(Id));
        }

    }
}