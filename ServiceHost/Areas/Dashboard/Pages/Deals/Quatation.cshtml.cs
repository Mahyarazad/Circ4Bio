using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Domain.NegotiateAggregate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Deals
{
    public class QuatationModel : PageModel
    {
        public DealViewModel Command;
        public SelectList CurrencyList;
        private readonly IDealApplication _dealApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public QuatationModel(IListingApplication listingApplication
            , IAutenticateHelper autenticateHelper,
            INegotiateApplication negotiateApplication,
            IDealApplication dealApplication)
        {
            _dealApplication = dealApplication;
            _autenticateHelper = autenticateHelper;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            var LoggedUser = _autenticateHelper.CurrentAccountRole().Id;
            var Negotiate = await _negotiateApplication.GetNegotiationViewModel(Id);

            if (Negotiate.SellerId == LoggedUser | Negotiate.BuyerId == LoggedUser)
            {
                Command = await _dealApplication.GetDealWithDealId(Id);
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

    }
}
