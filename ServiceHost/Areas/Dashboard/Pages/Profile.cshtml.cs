using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class ProfileModel : PageModel
    {
        public EditUser user;
        public SelectList CountrlyList;
        public string Role;
        public int NotificationCount;
        public List<NotificationViewModel> Notifications;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationApplication _notificationApplication;
        public ProfileModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor,
            INotificationApplication notificationApplication
        )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _notificationApplication = notificationApplication;
        }

        public void OnGet(long Id)
        {
            user = _userApplication.GetDetail(Id);
            CountrlyList = new SelectList(GenerateCountryList.GetList());
            Notifications = _notificationApplication.GetAll(
                long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value));
            NotificationCount = _notificationApplication.CountUnread(Id);
        }

        public JsonResult OnPost(EditUser user)
        {
            var result = _userApplication.EditByUser(user);
            return new JsonResult(result);
        }
        public IActionResult OnPostMarkRead(long Id)
        {
            var result = _notificationApplication.MarkRead(Id);
            var reqUrl = _contextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;
            return Redirect(reqUrl);
        }
    }
}
