using System;
using System.Collections.Generic;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class IndexModel : PageModel
    {
        public List<NotificationViewModel> Command;
        private readonly INotificationApplication _notificationApplication;

        public IndexModel(INotificationApplication notificationApplication)
        {
            _notificationApplication = notificationApplication;
        }

        public void OnGet()
        {
            Command = _notificationApplication.GetAll();
        }

    }
}
