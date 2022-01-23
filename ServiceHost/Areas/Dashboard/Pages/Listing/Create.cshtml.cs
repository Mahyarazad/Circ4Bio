using System.Collections.Generic;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class CreateModel : PageModel
    {
        public List<NotificationViewModel> Notifications;
        public CreateListing Command;
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
        }

        public JsonResult OnPost(CreateListing command)
        {
            var result = _listingApplication.Create(command);
            return new JsonResult(result);
        }

    }
}
