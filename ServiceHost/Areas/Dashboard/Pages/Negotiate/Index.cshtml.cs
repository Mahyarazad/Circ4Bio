using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var buyyingNegotiation = _negotiateApplication.AllListingItemsBuyyer(userId);
            var sellingNegotiation = _negotiateApplication.AllListingItemsSeller(userId);
            foreach (var item in buyyingNegotiation)
            {
                NegotiateList.Add(_negotiateApplication.GetNegotiationViewModel(new CreateNegotiate
                {
                    NegotiateId = item.NegotiateId,
                    ListingId = item.ListingId,
                    BuyerId = userId,
                    SellerId = item.SellerId
                }));
            }
            foreach (var item in sellingNegotiation)
            {
                NegotiateList.Add(_negotiateApplication.GetNegotiationViewModel(new CreateNegotiate
                {
                    NegotiateId = item.NegotiateId,
                    ListingId = item.ListingId,
                    BuyerId = item.BuyerId,
                    SellerId = userId
                }));
            }
        }
    }
}
