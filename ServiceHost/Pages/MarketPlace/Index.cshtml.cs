using System;
using System.Collections.Generic;
using System.Net.Http;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Domain.NaceAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.MarketPlace
{
    public class IndexModel : PageModel
    {
        public List<ListingViewModel> Command;
        public ListingViewModel Item;
        public List<NaceViewModel> NaceList;
        private readonly INaceApplication _naceApplication;
        private readonly IListingApplication _listingApplication;


        public IndexModel(INaceApplication naceApplication, IListingApplication listingApplication)
        {
            _naceApplication = naceApplication;
            _listingApplication = listingApplication;
        }

        public void OnGet()
        {
            Command = _listingApplication.GetAllListing().Result;
            NaceList = _naceApplication.GetAllNaceTitles().Result;
        }
        public IActionResult OnGetLoad(long Id)
        {
            Item = _listingApplication.GetDetailListing(Id).Result;
            Item.Slug = Slugify.GenerateSlug(Item.Name);
            return Partial("./MarketListingItem", Item);
        }
    }
}
