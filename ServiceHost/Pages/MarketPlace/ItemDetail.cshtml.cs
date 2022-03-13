using AM.Application.Contracts.Listing;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.MarketPlace
{
    public class ItemDetail : PageModel
    {
        public ListingViewModel Listing;
        private readonly IListingApplication _listingApplication;


        public ItemDetail(IListingApplication listingApplication)
        {
            _listingApplication = listingApplication;
        }

        public async void OnGet(long Id)
        {
            Listing = await _listingApplication.GetDetailListing(Id);
        }
    }
}
