using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Deals
{
    public class CreateModel : PageModel
    {
        public CreateDeal Command;
        public AuthViewModel LoggedUser;
        public SelectList CurrencyList;
        public SelectList DeliveryLocationSelectList;
        public SelectList DeliveryMethod;
        public SelectList DeliveryCharges;
        private readonly IDealApplication _dealApplication;
        private readonly IUserApplication _userApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public CreateModel(IListingApplication listingApplication
            , IUserApplication userApplication
            , IAuthenticateHelper authenticateHelper,
            INegotiateApplication negotiateApplication,
            IDealApplication dealApplication)
        {
            _userApplication = userApplication;
            _dealApplication = dealApplication;
            _authenticateHelper = authenticateHelper;
            _listingApplication = listingApplication;
            _negotiateApplication = negotiateApplication;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            LoggedUser = _authenticateHelper.CurrentAccountRole();
            var Negotiate = _negotiateApplication.GetNegotiationViewModel(Id);

            if (Negotiate.SellerId == LoggedUser.Id)
            {
                Command = new CreateDeal();
                CurrencyList = new SelectList(GenerateCurrencyList.GetList());
                Command.Listing = await _listingApplication
                    .GetDetailListing(Negotiate.ListingId);
                Command.NegotiateId = Id;
                Command.ListingId = Command.Listing.Id;
                DeliveryCharges = new SelectList(new List<string>
                {
                    new string("Buyer"),
                    new string("Seller"),
                });
                var ListView =
                    await _userApplication.GetDeliveryLocationDropDown(_authenticateHelper.CurrentAccountRole().Id);
                DeliveryLocationSelectList = new SelectList(ListView, "LocationId", "Name");
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public async Task<IActionResult> OnPost(CreateDeal Command)
        {
            Command.Listing = await _listingApplication
                .GetDetailListing(_negotiateApplication
                    .GetNegotiationViewModel(Command.NegotiateId).ListingId);
            Command.TotalCost = (Command.Amount * Command.Listing.UnitPrice) + Command.DeliveryCost;
            var result = await _dealApplication.CreateQuotation(Command);

            return RedirectToPage("/Deals/Index", new { Id = _authenticateHelper.CurrentAccountRole().Id });
        }
    }
}
