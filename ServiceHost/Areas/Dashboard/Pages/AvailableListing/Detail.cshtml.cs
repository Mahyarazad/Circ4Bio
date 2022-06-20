using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.NaceData;
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
        public NaceDataViewModel NaceData;
        public NaceViewModel NaceViewModel;

        private readonly IUserApplication _userApplication;
        private readonly INaceApplication _naceApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;
        private readonly INaceDataApplication _naceDataApplication;
        private readonly INegotiateApplication _negotiateApplication;


        public Detail(IUserApplication userApplication, INaceApplication naceApplication, IHttpContextAccessor contextAccessor, IListingApplication listingApplication, INaceDataApplication naceDataApplication, INegotiateApplication negotiateApplication)
        {
            _userApplication = userApplication;
            _naceApplication = naceApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
            _naceDataApplication = naceDataApplication;
            _negotiateApplication = negotiateApplication;
        }

        public async void OnGet(long Id)
        {
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = await _userApplication.GetDetail(userId);
            Listing = await _listingApplication.GetDetailListing(Id);
            NaceData = _naceDataApplication.GetNaceData(Listing.Id);
            NaceViewModel = _naceApplication.GetSingleNace(NaceData.NaceId).Result;
            var naceSelectListStringData =
                NaceViewModel.Items.Where(x => x.ListItems.Count > 1).ToList();

            foreach (var item in NaceData.NaceDataDetails.Select((value, index) => new { value, index }))
            {
                if (item.value.ItemdetailValues == "")
                {
                    naceSelectListStringData.ForEach(x => x.ListItems.ForEach(y =>
                      {
                          if (y.ListItemDetailId == item.value.ItemdetailIndex)
                              item.value.ItemdetailValues = y.ListItemDetail;
                      }));
                }
            }
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
