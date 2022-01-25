using AM.Application.Contracts.ContactUs;
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

        public void OnGet(long id)
        {
            Message = _contactUsApplication.GetSingleMessages(id);
        }
        public JsonResult OnPostIsReed(long id)
        {
            return new JsonResult(_contactUsApplication.MarkAsRead(id));
        }
    }
}
