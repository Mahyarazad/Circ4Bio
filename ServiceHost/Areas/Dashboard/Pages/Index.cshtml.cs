using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationApplication _notificationApplication;
        public List<NotificationViewModel> Command;

        public IndexModel(IHttpContextAccessor contextAccessor, INotificationApplication notificationApplication)
        {
            _contextAccessor = contextAccessor;
            _notificationApplication = notificationApplication;
        }

        public void OnGet()
        {
            Command = _notificationApplication.GetAll(
                long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value));
        }
        public IActionResult OnPostMarkRead(long Id)
        {
            var result = _notificationApplication.MarkRead(Id);
            var reqUrl = _contextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;
            return Redirect(reqUrl);
        }
    }
}