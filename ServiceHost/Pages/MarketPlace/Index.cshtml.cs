using System;
using System.Collections.Generic;
using System.Net.Http;
using AM.Application.Contracts.Listing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.MarketPlace
{
    public class IndexModel : PageModel
    {
        public List<ListingViewModel> Command;
        public ListingViewModel Item;
        private readonly IListingApplication _listingApplication;

        public IndexModel(IListingApplication listingApplication)
        {
            _listingApplication = listingApplication;
        }

        public void OnGet()
        {
            Command = _listingApplication.GetAllListing().Result;
        }
        public IActionResult OnGetLoad(long Id)
        {
            Item = _listingApplication.GetDetailListing(Id).Result;
            return Partial("./MarketListingItem", Item);
        }
    }
}
