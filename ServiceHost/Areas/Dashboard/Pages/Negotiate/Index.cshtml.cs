using System.Collections.Generic;
using System.Linq;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Negotiate
{
    public class IndexModel : PageModel
    {
        public List<NegotiateViewModel> NegotiateList;
        public List<ActiveListing> ActiveListing;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public IndexModel(IListingApplication listingApplication,
            INegotiateApplication negotiateApplication,
            IUserApplication userApplication,
            IHttpContextAccessor contextAccessor)
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public void OnGet()
        {
            NegotiateList = new List<NegotiateViewModel>();
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);
            var buyyingnegotiation = _negotiateApplication.AllListingItems(userId);
            foreach (var item in buyyingnegotiation)
            {
                NegotiateList.Add(_negotiateApplication.GetNegotiationList(new CreateNegotiate
                {
                    ListingId = item.ListingId,
                    BuyyerId = userId,
                    SellerId = item.SellerId
                }));
            }
        }
    }
}
