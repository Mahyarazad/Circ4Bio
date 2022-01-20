using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        public List<ListingViewModel> Listing;
        public bool ShowDeleted = false;
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
            ShowDeleted = false;

            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = _userApplication.GetDetail(userId);

            var AdminCheck = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (AdminCheck == "1")
            {
                Listing = _listingApplication.GetAllListing();
            }
            else
            {
                Listing = _listingApplication.GetUserListing(user.Id);
            }

        }

        public void OnGetDeleted()
        {
            ShowDeleted = true;
            user = _userApplication.GetDetail(
                long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value));

            Listing = _listingApplication.GetDeletedUserListing(user.Id);
        }

        public JsonResult OnPostMarkDelete(long id)
        {
            var result = _listingApplication.Delete(id);
            return new JsonResult(result);
        }
        public JsonResult OnPostMarkPublic(long id)
        {
            var result = _listingApplication.MarkPublic(id);
            return new JsonResult(result);
        }
        public JsonResult OnPostMarkPrivate(long id)
        {
            var result = _listingApplication.MarkPrivate(id);
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
