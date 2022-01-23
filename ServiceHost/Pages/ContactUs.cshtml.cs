using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.ContactUs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class ContactUsModel : PageModel
    {
        private readonly IContactUsApplication _contactUsApplication;
        public CreateMessage Command;

        public ContactUsModel(IContactUsApplication contactUsApplication)
        {
            _contactUsApplication = contactUsApplication;
        }

        [TempData] public string Message { get; set; }
        [TempData] public string ErrorMessage { get; set; }
        public void OnGet()
        {

        }
        public IActionResult OnPost(CreateMessage command)
        {
            Message = "";
            ErrorMessage = "";
            var result = _contactUsApplication.CreateMessage(command);
            if (result.IsSucceeded)
            {
                Message = result.Message;
                return RedirectToPage("ContactUs");
            }
            ErrorMessage = result.Message;
            return RedirectToPage("ContactUs");
        }
    }
}
