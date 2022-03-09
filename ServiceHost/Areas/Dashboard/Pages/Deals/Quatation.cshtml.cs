using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
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
        public SelectList DeliveryCharges;
        public SelectList DeliveryLocationSelectList;
        public AuthViewModel LoggedUser;
        private readonly IUserApplication _userApplication;
        private readonly IDealApplication _dealApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public QuatationModel(IListingApplication listingApplication
            , IAuthenticateHelper authenticateHelper,
            INegotiateApplication negotiateApplication,
            IUserApplication userApplication,
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
            CurrencyList = new SelectList(GenerateCurrencyList.GetList());
            if (Negotiate.SellerId == LoggedUser.Id | Negotiate.BuyerId == LoggedUser.Id)
            {
                Command = _dealApplication.GetDealWithNegotiateId(Id);
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

        public JsonResult OnPost(DealViewModel Command)
        {
            return new JsonResult(_dealApplication.EditDeal(Command));
        }
    }
}
