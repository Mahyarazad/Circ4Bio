using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Application.PayPal;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace ServiceHost.Areas.Dashboard.Pages.Deals
{
    public class ConfirmQuatationModel : PageModel
    {
        public DealViewModel Command;
        public SelectList CurrencyList;
        public SelectList DeliveryCharges;
        public SelectList DeliveryLocationSelectList;
        public AuthViewModel LoggedUser;

        private readonly IPayPalService _payPalService;
        private readonly IConfiguration _configuration;
        private readonly IUserApplication _userApplication;
        private readonly IDealApplication _dealApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INegotiateApplication _negotiateApplication;

        public ConfirmQuatationModel(IListingApplication listingApplication
            , IAuthenticateHelper authenticateHelper,
            IHttpContextAccessor contextAccessor,
            IPayPalService payPalService,
            IConfiguration configuration,
            INegotiateApplication negotiateApplication,
            IUserApplication userApplication,
            IDealApplication dealApplication)
        {
            _configuration = configuration;
            _payPalService = payPalService;
            _userApplication = userApplication;
            _dealApplication = dealApplication;
            _contextAccessor = contextAccessor;
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
                DeliveryLocationSelectList =
                    new SelectList(await _userApplication.GetDeliveryLocationDropDown(_authenticateHelper.CurrentAccountRole().Id));
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public JsonResult OnPost(DealViewModel Command)
        {

            return new JsonResult(_dealApplication.AtivateDeal(Command));
        }

        public async Task<IActionResult> OnGetCheckOut(long Id)
        {
            var deal = _dealApplication.GetDealWithNegotiateId(Id);
            var url = await _payPalService.GetRedirectUrltoPayPal(deal.TotalCost, deal.Currency, deal.TrackingCode);
            if (url != null)
            {
                return Redirect(url);
            }
            else
            {
                return Redirect("/dashboard/paypalerror");
            }

        }

    }
}
