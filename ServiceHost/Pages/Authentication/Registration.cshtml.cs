using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Authentication
{
    public class RegistrationIndex : PageModel
    {
        public RegisterUser Command;
        [TempData] public string RegisterMessage { get; set; }
        [TempData] public string RegisterSuccess { get; set; }

        public RegistrationIndex(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        private readonly IUserApplication _userApplication;
        public void OnGet()
        {
            TempData.Remove("RegisterSuccess");
        }

        public IActionResult OnPostRegister(RegisterUser command)
        {
            var result = _userApplication.Register(command);

            if (result.IsSucceeded)
            {
                TempData["RegisterSuccess"] = ApplicationMessage.SuccessfulRegister;
                return RedirectToPage("/Authentication/Registration");
            }
            TempData["RegisterMessage"] = result.Message;
            return RedirectToPage("/Authentication/Registration");
        }
    }
}
