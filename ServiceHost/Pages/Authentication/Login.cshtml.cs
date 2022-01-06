using _0_Framework;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class LoginIndex : PageModel
    {
        public EditUser Command;
        [TempData] public string LoginMessage { get; set; }
        [TempData] public string RegisterMessage { get; set; }
        [TempData] public string RegisterSuccess { get; set; }

        public LoginIndex(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        private readonly IUserApplication _userApplication;
        public void OnGet()
        {

        }

        public IActionResult OnGetLogout()
        {
            _userApplication.Logout();
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostLogin(EditUser command)
        {
            var result = _userApplication.Login(command);
            if (result.IsSucceeded)
            {
                RegisterSuccess = ApplicationMessage.SuccessLogin;
                return RedirectToPage("./Index", new { area = "Dashboard" });
            }
            RegisterMessage = result.Message;
            return RedirectToPage("./Index");
        }


    }
}
