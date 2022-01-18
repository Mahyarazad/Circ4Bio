using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class IndexModel : PageModel
    {
        public EditUser user;
        public SelectList CountrlyList;
        public string Role;
        public List<NotificationViewModel> Notifications;
        public List<ListingViewModel> Listing;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationApplication _notificationApplication;
        private readonly IListingApplication _listingApplication;
        public IndexModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor,
            INotificationApplication notificationApplication,
            IListingApplication listingApplication
        )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _notificationApplication = notificationApplication;
        }

        public void OnGet()
        {
            user = _userApplication.GetDetail(
                long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value));
            Notifications = _notificationApplication.GetAll(
                long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value));
            Listing = _listingApplication.GetUserListing(user.Id);
        }

    }
}
