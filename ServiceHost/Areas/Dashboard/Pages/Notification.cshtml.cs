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
        private readonly IAutenticateHelper _autenticateHelper;
        public NotificationModel(INotificationApplication notificationApplication,
            IAutenticateHelper autenticateHelper)
        {
            _notificationApplication = notificationApplication;
            _autenticateHelper = autenticateHelper;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            var loggedInUserId = _autenticateHelper.CurrentAccountRole().Id;

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
        public async Task OnGetMarkAllRead(long Id)
        {
            _notificationApplication.MarkAllRead(Id);
            Command = await _notificationApplication.GetAllUnread(Id);
        }
        public JsonResult OnPostMarkRead(long Id)
        {
            return new JsonResult(_notificationApplication.MarkRead(Id));
        }

    }
}