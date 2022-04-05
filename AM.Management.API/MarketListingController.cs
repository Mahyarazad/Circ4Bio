using System;
using System.Collections.Generic;
using AM.Application.Contracts.Listing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketListingController : ControllerBase
    {
        private readonly IListingApplication _listingApplication;

        public MarketListingController(IListingApplication listingApplication)
        {
            _listingApplication = listingApplication;
        }

        [HttpGet]
        public List<ListingViewModel> OnGet()
        {
            return _listingApplication.GetAllListing().Result;
        }
    }
}