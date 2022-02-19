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

        public async void OnGet()
        {
            ShowDeleted = false;

            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = await _userApplication.GetDetail(userId);

            var AdminCheck = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (AdminCheck == "1")
            {
                Listing = await _listingApplication.GetAllListing();
            }
            else
            {
                Listing = await _listingApplication.GetUserListing(user.Id);
            }

        }
        public async void OnGetDeleted()
        {
            ShowDeleted = true;
            user = await _userApplication.GetDetail(
                long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value));

            Listing = await _listingApplication.GetDeletedUserListing(user.Id);
        }
        public async void OnGetHideDeletedForAdmin()
        {
            user = await _userApplication.GetDetail(
                long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value));
            Listing = await _listingApplication.GetAllListing();
            Listing = Listing.Where(x => !x.IsDeleted).ToList();
        }

        public async void OnGetShowDeletedForAdmin()
        {
            user = await _userApplication.GetDetail(
                long.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value));
            Listing = await _listingApplication.GetAllListing();
        }

        public JsonResult OnPostMarkDelete(long id)
        {
            return new JsonResult(_listingApplication.Delete(id));
        }
        public JsonResult OnPostMarkPublic(long id)
        {
            return new JsonResult(_listingApplication.MarkPublic(id));
        }
        public JsonResult OnPostMarkPrivate(long id)
        {
            return new JsonResult(_listingApplication.MarkPrivate(id));
        }
        public IActionResult OnPostMarkRead(long Id)
        {
            var result = _notificationApplication.MarkRead(Id);
            var reqUrl = _contextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;
            return Redirect(reqUrl);
        }

        public IActionResult OnGetIncrement(long id)
        {
            var inputAmount = new InputAmount()
            {
                ListingId = id
            };
            return Partial("./Increment", inputAmount);
        }
        public IActionResult OnGetDecrement(long id)
        {
            var inputAmount = new InputAmount()
            {
                ListingId = id
            };
            return Partial("./Decrement", inputAmount);
        }

        public JsonResult OnPostIncrement(InputAmount command)
        {
            return new JsonResult(_listingApplication.IncrementAmount(command));
        }
        public JsonResult OnPostDecrement(InputAmount command)
        {
            return new JsonResult(_listingApplication.DeccrementAmount(command));
        }
        public IActionResult OnGetLog(long id)
        {
            var cds = _listingApplication.GetListingOperationLog(id);
            return Partial("./Log", _listingApplication.GetListingOperationLog(id));
        }
    }
}
