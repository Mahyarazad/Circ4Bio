using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class ResetPasswordIndex : PageModel
    {
        private readonly IUserApplication _userApplication;
        private readonly IResetPasswordApplication _resetPasswordApplication;
        public ResetPasswordModel Command { get; set; }
        public ResetPasswordIndex(IResetPasswordApplication resetPasswordApplication,
            IUserApplication userApplication)
        {
            _userApplication = userApplication;
            _resetPasswordApplication = resetPasswordApplication;
        }

        public void OnGet(long Id)
        {
            Command = new ResetPasswordModel();
            Command.UserId = Id;
        }


        public JsonResult OnPost(ResetPasswordModel command)
        {
            var result = _userApplication.ResetPassword(command);
            return new JsonResult(result);
        }
    }
}