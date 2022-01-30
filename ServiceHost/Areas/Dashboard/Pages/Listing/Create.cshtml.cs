using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;
        public CreateModel(
            IHttpContextAccessor contextAccessor,
            IListingApplication listingApplication)
        {
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
        }

        public void OnGet()
        {
            CurrencyList = new SelectList(GenerateCurrencyList.GetList());
        }

        public JsonResult OnPost(CreateListing command)
        {
            return new JsonResult(_listingApplication.Create(command));
        }

    }
}
