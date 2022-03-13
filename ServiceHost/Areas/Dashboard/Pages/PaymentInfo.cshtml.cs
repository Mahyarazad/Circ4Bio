using _0_Framework.Application;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class PaymentInfo : PageModel
    {
        private readonly IDealApplication _dealApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;
        public DealViewModel Command;
        public AuthViewModel LoggedUser;


        public PaymentInfo(IDealApplication dealApplication, IAuthenticateHelper authenticateHelper, INegotiateApplication negotiateApplication)
        {
            _dealApplication = dealApplication;
            _authenticateHelper = authenticateHelper;
            _negotiateApplication = negotiateApplication;
        }

        public IActionResult OnGet(long Id)
        {
            LoggedUser = _authenticateHelper.CurrentAccountRole();
            Command = _dealApplication.GetDealWithDealId(Id);
            var Negotiate = _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
            if (Negotiate.SellerId == LoggedUser.Id | Negotiate.BuyerId == LoggedUser.Id)
            {
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied");
            }
        }
    }
}