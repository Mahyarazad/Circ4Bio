using _0_Framework.Application;
using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class ResetPasswordIndex : PageModel
    {
        private readonly IUserApplication _userApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        public ResetPasswordModel Command { get; set; }
        public ResetPasswordIndex(IAuthenticateHelper authenticateHelper,
            IUserApplication userApplication)
        {
            _userApplication = userApplication;
            _authenticateHelper = authenticateHelper;
        }

        public IActionResult OnGet(long Id)
        {
            if (Id == _authenticateHelper.CurrentAccountRole().Id)
            {
                Command = new ResetPasswordModel();
                Command.UserId = Id;
                return null;
            }
            else
            {
                return RedirectToPage("AccessDenied", new { area = "" });
            }
        }


        public JsonResult OnPost(ResetPasswordModel command)
        {
            var result = _userApplication.ResetPassword(command);
            return new JsonResult(result);
        }
    }
}