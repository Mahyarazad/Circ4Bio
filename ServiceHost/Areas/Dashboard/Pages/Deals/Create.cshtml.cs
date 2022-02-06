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
    public class CreateModel : PageModel
    {
        public CreateDeal Command;
        public SelectList CurrencyList;
        private readonly IDealApplication _dealApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public CreateModel(IListingApplication listingApplication
            , IAutenticateHelper autenticateHelper,
            INegotiateApplication negotiateApplication,
            IDealApplication dealApplication)
        {
            _dealApplication = dealApplication;
            _autenticateHelper = autenticateHelper;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public IActionResult OnGet(long Id)
        {
            var LoggedUser = _autenticateHelper.CurrentAccountRole().Id;
            var Negotiate = _negotiateApplication.GetNegotiationViewModel(Id);

            if (Negotiate.SellerId == LoggedUser)
            {
                Command = new CreateDeal();
                CurrencyList = new SelectList(GenerateCurrencyList.GetList());
                Command.Listing = _listingApplication
                    .GetDetailListing(Negotiate.ListingId);
                Command.NegotiateId = Id;
                Command.ListingId = Command.Listing.Id;
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public IActionResult OnPost(CreateDeal Command)
        {
            Command.Listing = _listingApplication
                .GetDetailListing(_negotiateApplication
                    .GetNegotiationViewModel(Command.NegotiateId).ListingId);
            Command.TotalCost = (Command.Amount * Command.Listing.UnitPrice) + Command.DeliveryCost;
            var result = _dealApplication.CreateDeal(Command);

            return RedirectToPage("/Deals/Index", new { Id = _autenticateHelper.CurrentAccountRole().Id });
        }
    }
}
