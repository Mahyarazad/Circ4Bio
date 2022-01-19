using System.Collections.Generic;
using System.Linq;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class CreateModel : PageModel
    {
        public List<NotificationViewModel> Notifications;
        public CreateListing Command;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationApplication _notificationApplication;
        private readonly IListingApplication _listingApplication;
        public CreateModel(
            IHttpContextAccessor contextAccessor,
            INotificationApplication notificationApplication,
            IListingApplication listingApplication)
        {
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _notificationApplication = notificationApplication;
        }

        public void OnGet()
        {
            Notifications = _notificationApplication.GetAll(
                long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value));
        }

        public JsonResult OnPost(CreateListing command)
        {
            var result = _listingApplication.Create(command);
            return new JsonResult(result);
        }

    }
}
