using System.Threading.Tasks;
using _0_Framework;
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

        }

        public async Task<IActionResult> OnPostRegister(RegisterUser command)
        {
            RegisterSuccess = "";
            RegisterMessage = "";
            var result = await _userApplication.Register(command);
            if (result.IsSucceeded)
            {
                RegisterSuccess = ApplicationMessage.SuccessfulRegister;
                return RedirectToPage("/Authentication/Registration");
            }
            RegisterMessage = result.Message;
            return RedirectToPage("/Authentication/Registration");
        }
    }
}
