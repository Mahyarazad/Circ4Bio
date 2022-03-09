using System.Collections.Generic;
using System.Threading.Tasks;
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
        public DealViewModel Command;
        private readonly IDealApplication _dealApplication;
        private readonly IUserApplication _userApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        public IndexModel(IUserApplication userApplication,
            IAuthenticateHelper authenticateHelper,
            IDealApplication dealApplication,
            IListingApplication listingApplication
        )
        {
            _dealApplication = dealApplication;
            _userApplication = userApplication;
            _authenticateHelper = authenticateHelper;
            _listingApplication = listingApplication;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            if (Id == _authenticateHelper.CurrentAccountRole().Id)
            {
                Deals = await _dealApplication.GetAllDeals(Id);
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public JsonResult OnPost(long id)
        {
            Command = _dealApplication.GetDealWithDealId(id);
            var result = _dealApplication.AtivateDeal(Command).Result;
            return new JsonResult(Task.FromResult(result));
        }
    }
}
