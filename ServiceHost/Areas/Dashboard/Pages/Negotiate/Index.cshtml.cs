using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Negotiate
{
    public class IndexModel : PageModel
    {
        public List<NegotiateViewModel> NegotiateList;
        public List<ActiveListing> ActiveListing;
        public CreateNegotiate NegotiateDTO;
        private readonly IUserApplication _userApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public IndexModel(IListingApplication listingApplication,
            INegotiateApplication negotiateApplication,
            IUserApplication userApplication,
            IAutenticateHelper autenticateHelper)
        {
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public async Task OnGet()
        {
            NegotiateList = new List<NegotiateViewModel>();
            var userId = _autenticateHelper.CurrentAccountRole().Id;
            var buyyingNegotiation = await _negotiateApplication.AllListingItemsBuyyer(userId);
            var sellingNegotiation = await _negotiateApplication.AllListingItemsSeller(userId);
            foreach (var item in buyyingNegotiation)
            {
                NegotiateList.Add(await _negotiateApplication.GetNegotiationViewModel(new CreateNegotiate
                {
                    NegotiateId = item.NegotiateId,
                    ListingId = item.ListingId,
                    BuyerId = userId,
                    SellerId = item.SellerId,
                    IsCanceled = item.IsCanceled,
                    IsFinished = item.IsFinished,
                    IsActive = item.IsActive
                }));
            }
            foreach (var item in sellingNegotiation)
            {
                NegotiateList.Add(await _negotiateApplication.GetNegotiationViewModel(new CreateNegotiate
                {
                    NegotiateId = item.NegotiateId,
                    ListingId = item.ListingId,
                    BuyerId = item.BuyerId,
                    SellerId = userId,
                    IsCanceled = item.IsCanceled,
                    IsFinished = item.IsFinished,
                    IsActive = item.IsActive
                }));
            }
        }

        public JsonResult OnPostCancelNegotiation(CreateNegotiate Command)
        {
            var targetNegotiate = new CreateNegotiate
            {
                NegotiateId = Command.NegotiateId,
                BuyerId = Command.BuyerId,
                SellerId = Command.SellerId,
                ListingId = Command.ListingId
            };
            return new JsonResult(_negotiateApplication.CancelNegotiation(targetNegotiate));
        }
    }
}
