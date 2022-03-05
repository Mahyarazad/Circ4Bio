using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.AvailableListing
{
    public class Detail : PageModel
    {
        public EditUser user;
        public ListingViewModel Listing;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public Detail(IListingApplication listingApplication,
            INegotiateApplication negotiateApplication,
            IUserApplication userApplication,
            IHttpContextAccessor contextAccessor)
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public async void OnGet(long Id)
        {
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = await _userApplication.GetDetail(userId);
            Listing = await _listingApplication.GetDetailListing(Id);
        }

        public async Task<JsonResult> OnPost(long Id)
        {
            var createNegotiation = new CreateNegotiate
            {
                ListingId = Id,
                BuyerId = long.Parse(_contextAccessor.HttpContext.User.Claims
                    .FirstOrDefault(x => x.Type == "User Id").Value),
                SellerId = await _listingApplication.GetOwnerUserID(Id)
            };
            var res = await _negotiateApplication.Create(createNegotiation);
            return new JsonResult(Task.FromResult(res));
        }
    }
}
