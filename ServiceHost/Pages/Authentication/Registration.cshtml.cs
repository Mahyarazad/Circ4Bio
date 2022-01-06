using _0_Framework;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class RegistrationIndex : PageModel
    {
        public RegisterUser Command;
        [TempData] public string LoginMessage { get; set; }
        [TempData] public string RegisterMessage { get; set; }
        [TempData] public string RegisterSuccess { get; set; }

        public RegistrationIndex(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        private readonly IUserApplication _userApplication;
        public void OnGet()
        {

        }

        public IActionResult OnPostRegister(RegisterUser command)
        {
            var result = _userApplication.Register(command);
            if (result.IsSucceeded)
            {
                RegisterSuccess = ApplicationMessage.SuccessfulRegister;
                return RedirectToPage("./Authenticate");
            }
            RegisterMessage = result.Message;
            return RedirectToPage("./Index");
        }

        
    }
}
