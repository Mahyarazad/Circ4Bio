using _0_Framework;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Authentication
{
    public class ActivateUserModel : PageModel
    {
        public string RegisterMessage;
        public string RegisterSuccess;
        private readonly IUserApplication _userApplication;

        public ActivateUserModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public void OnGet(string id)
        {
            var result = _userApplication.ActivateUser(id).Result;
            if (result.IsSucceeded)
            {
                RegisterSuccess = ApplicationMessage.SuccessfulActivation;
            }
            else
            {
                RegisterMessage = result.Message;
            }
        }
    }
}
