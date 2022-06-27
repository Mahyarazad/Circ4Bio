using System.Threading.Tasks;
using _0_Framework;
using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Authentication
{
    public class ResendActivationEmailIndex : PageModel
    {
        private readonly IUserApplication _userApplication;
        public ResetPasswordModel Command;
        public ResendActivationEmailIndex(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }
        [TempData] public string Message { get; set; }
        [TempData] public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(ResendActivationEmail command)
        {
            var result = await _userApplication.SendActivationEmail(command.Email);
            if (result.IsSucceeded)
            {
                TempData["SuccessMessage"] = ApplicationMessage.SuccessfulRegister;
                return RedirectToPage("/Authentication/ResendActivationEmail", new { area = "" });
            }
            else
            {
                TempData["Message"] = result.Message;
                return RedirectToPage("/Authentication/ResendActivationEmail", new { area = "" });
            }
        }
    }
}
