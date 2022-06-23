using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.ContactUs;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.ContactUs
{
    public class MessageBodyModel : PageModel
    {
        private readonly IContactUsApplication _contactUsApplication;
        public ContactUsViewModel Message;
        public MessageBodyModel(IContactUsApplication contactUsApplication)
        {
            _contactUsApplication = contactUsApplication;
        }

        [NeedsPermission(UserPermission.GetContactUsMessages)]
        public async void OnGet(long id)
        {
            Message = await _contactUsApplication.GetSingleMessages(id);
        }

        [NeedsPermission(UserPermission.MarkAsReedMessages)]
        public JsonResult OnPostIsReed(long id)
        {
            return new JsonResult(_contactUsApplication.MarkAsRead(id));
        }
    }
}
