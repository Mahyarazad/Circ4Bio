using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework.Application;
using AM.Application.Contracts.ContactUs;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.ContactUs
{
    public class IndexModel : PageModel
    {
        public EditUser user;

        private readonly IContactUsApplication _contactUsApplication;
        public ContactUsViewModel Command;
        public bool IsReed { get; set; }
        public List<ContactUsViewModel> ContactUsMessagList;
        [TempData]
        public string Message { get; set; }

        public IndexModel(IContactUsApplication contactUsApplication)
        {
            _contactUsApplication = contactUsApplication;
        }
        [RequirePermission(UserPermission.GetContactUsMessages)]
        public void OnGet()
        {
            ContactUsMessagList = _contactUsApplication.GetContactUsMessages().Result;
        }
        [RequirePermission(UserPermission.GetContactUsMessages)]
        public void OnPostGetReedMessages(bool IsReed)
        {
            if (IsReed)
            {

                ContactUsMessagList = _contactUsApplication.GetReadContactUsMessages().Result;
            }
            else
            {
                ContactUsMessagList = _contactUsApplication.GetContactUsMessages().Result;
            }
        }
        [RequirePermission(UserPermission.MarkAsReedMessages)]
        public JsonResult OnPostIsReed(long id)
        {
            return new JsonResult(_contactUsApplication.MarkAsRead(id));
        }
    }
}
