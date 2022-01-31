using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.AvailableListing
{
    public class AvailableListingModel : PageModel
    {
        public EditUser user;
        public List<ListingViewModel> Listing;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public AvailableListingModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor,
            INegotiateApplication negotiateApplication,
            IListingApplication listingApplication
        )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public void OnGet()
        {
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = _userApplication.GetDetail(userId);
            Listing = _listingApplication.GetAllPublicListing();
        }
        public IActionResult OnPost(long Id)
        {
            var createNegotiation = new CreateNegotiate
            {
                ListingId = Id,
                BuyyerId = long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value),
                SellerId = _listingApplication.GetOwnerUserID(Id)
            };

            _negotiateApplication.Create(createNegotiation);
            return RedirectToPage("/Negotiate/Index", new { area = "Dashboard" });
        }
    }
}
