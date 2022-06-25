using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.AvailableListing
{

    public class AvailableListingModel : PageModel
    {
        public EditUser user;
        public List<ListingViewModel> Listing;
        public ListingViewModel Item;
        public List<NaceViewModel> NaceList;
        private readonly INaceApplication _naceApplication;
        private readonly IUserApplication _userApplication;

        public AvailableListingModel(INaceApplication naceApplication, IUserApplication userApplication, IHttpContextAccessor contextAccessor, IListingApplication listingApplication, INegotiateApplication negotiateApplication)
        {
            _naceApplication = naceApplication;
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public async Task OnGet()
        {
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = await _userApplication.GetDetail(userId);
            Listing = await _listingApplication.GetAllListing();
            NaceList = await _naceApplication.GetAllNaceTitles();
        }

        [NeedsPermission(UserPermission.CreateNegotiation)]
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

        public IActionResult OnGetLoad(long Id)
        {
            Item = _listingApplication.GetDetailListing(Id).Result;
            return Partial("./DetailModal", Item);
        }
    }
}
