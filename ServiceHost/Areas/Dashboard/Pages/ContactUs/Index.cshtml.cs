using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.ContactUs;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.ContactUs
{
    public class IndexModel : PageModel
    {
        public EditUser user;

        private readonly IContactUsApplication _contactUsApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        public ContactUsViewModel Command;
        public List<ContactUsViewModel> ContactUsMessagList;
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string Message { get; set; }

        public IndexModel(IContactUsApplication contactUsApplication,
            IHttpContextAccessor contextAccessor
            )
        {
            _contactUsApplication = contactUsApplication;
            _contextAccessor = contextAccessor;
        }

        public void OnGet()
        {
            ContactUsMessagList = _contactUsApplication.GetContactUsMessages();
        }

        public JsonResult OnPostIsReed(long id)
        {
            var result = _contactUsApplication.MarkAsRead(id);
            return new JsonResult(result);
        }
        public void OnPostGetAll(ContactUsViewModel Command)
        {
            ContactUsMessagList = _contactUsApplication.GetAllContactUsMessages();
        }
        public void OnPostGetReadMessages(ContactUsViewModel Command)
        {
            ContactUsMessagList = _contactUsApplication.GetReadContactUsMessages();
        }
    }
}
