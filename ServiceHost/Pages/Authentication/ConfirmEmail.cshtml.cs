using System.Threading.Tasks;
using _0_Framework;
using AM.Application.Contracts.ResetPassword;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Authentication
{
    public class ConfirmEmailIndex : PageModel
    {
        private readonly IResetPasswordApplication _resetPasswordApplication;
        public ResetPasswordModel Command;
        public ConfirmEmailIndex(IResetPasswordApplication resetPasswordApplication)
        {
            _resetPasswordApplication = resetPasswordApplication;
        }

        [TempData] public string Message { get; set; }
        [TempData] public string SuccessMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(ResetPasswordModel command)
        {

            var result = await _resetPasswordApplication.CreateResetPassword(command.Email);
            if (result.IsSucceeded)
            {

                TempData["SuccessMessage"] = ApplicationMessage.ResetPasswordGuidance;
                return RedirectToPage("/Authentication/ConfirmEmail", new { area = "" });
            }
            TempData["Message"] = result.Message;
            return RedirectToPage("/Authentication/ConfirmEmail", new { area = "" });
        }
    }
}
