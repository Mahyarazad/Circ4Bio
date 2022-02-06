using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Deals
{
    public class IndexModel : PageModel
    {
        public List<DealViewModel> Deals;
        private readonly IDealApplication _dealApplication;
        private readonly IUserApplication _userApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IListingApplication _listingApplication;
        public IndexModel(IUserApplication userApplication,
            IAutenticateHelper autenticateHelper,
            IDealApplication dealApplication,
            IListingApplication listingApplication
        )
        {
            _dealApplication = dealApplication;
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
            _listingApplication = listingApplication;
        }

        public IActionResult OnGet(long Id)
        {
            if (Id == _autenticateHelper.CurrentAccountRole().Id)
            {
                Deals = _dealApplication.GetAllDeals(Id);
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }
    }
}
