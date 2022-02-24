using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class CreateModel : PageModel
    {
        public List<NotificationViewModel> Notifications;
        public CreateListing Command;
        public SelectList CurrencyList;
        public SelectList DeliveryCharges;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly IUserApplication _userApplication;
        public CreateModel(
            IAuthenticateHelper authenticateHelper,
            IUserApplication userApplication,
            IListingApplication listingApplication)
        {
            _authenticateHelper = authenticateHelper;
            _userApplication = userApplication;
            _listingApplication = listingApplication;
        }

        public void OnGet()
        {
            Command = new CreateListing();
            CurrencyList = new SelectList(GenerateCurrencyList.GetList());
            DeliveryCharges = new SelectList(new List<string>
            {
                new string("Buyer"),
                new string("Seller"),
            });

        }

        public JsonResult OnPost(CreateListing command)
        {
            return new JsonResult(_listingApplication.Create(command));
        }

    }
}
