using System.Collections.Generic;
using System.Dynamic;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class NotificationModel : PageModel
    {
        public List<NotificationViewModel> Command;
        private readonly INotificationApplication _notificationApplication;

        public NotificationModel(INotificationApplication notificationApplication)
        {
            _notificationApplication = notificationApplication;
        }

        public void OnGet(long Id)
        {
            Command = _notificationApplication.GetAllUnread(Id);
        }

        public JsonResult OnPostMarkRead(long Id)
        {
            return new JsonResult(_notificationApplication.MarkRead(Id));
        }

    }
}