using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
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
        private readonly IAuthenticateHelper _authenticateHelper;
        public NotificationModel(INotificationApplication notificationApplication,
            IAuthenticateHelper authenticateHelper)
        {
            _notificationApplication = notificationApplication;
            _authenticateHelper = authenticateHelper;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            var loggedInUserId = _authenticateHelper.CurrentAccountRole().Id;

            if (Id == loggedInUserId)
            {
                Command = await _notificationApplication.GetAllUnread(Id);
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }
        public Task<JsonResult> OnGetMarkAllRead(long Id)
        {
            var result = _notificationApplication.MarkAllRead(Id);
            Command = _notificationApplication.GetAllUnread(Id).Result;
            return Task.FromResult(new JsonResult(result));
        }
        public Task<JsonResult> OnPostMarkRead(long Id)
        {
            return Task.FromResult(new JsonResult(_notificationApplication.MarkRead(Id)));
        }

    }
}