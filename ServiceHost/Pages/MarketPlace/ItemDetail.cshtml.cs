using System.Linq;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.NaceData;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.MarketPlace
{
    public class ItemDetail : PageModel
    {
        public ListingViewModel Listing;
        public NaceDataViewModel NaceData;
        public NaceViewModel NaceViewModel;
        private readonly INaceApplication _naceApplication;
        private readonly IListingApplication _listingApplication;
        private readonly INaceDataApplication _naceDataApplication;

        public ItemDetail(INaceApplication naceApplication, IListingApplication listingApplication, INaceDataApplication naceDataApplication)
        {
            _naceApplication = naceApplication;
            _listingApplication = listingApplication;
            _naceDataApplication = naceDataApplication;
        }

        public async void OnGet(long Id)
        {
            Listing = await _listingApplication.GetDetailListing(Id);
            NaceData = _naceDataApplication.GetNaceData(Listing.Id);
            if (NaceData.Id == 0)
            {
                NaceViewModel = new NaceViewModel();
            }
            else
            {
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
        }
    }
}
