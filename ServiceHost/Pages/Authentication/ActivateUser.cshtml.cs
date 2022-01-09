using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages.Authentication
{
    public class ActivateUserModel : PageModel
    {
        [TempData] public string RegisterMessage { get; set; }
        [TempData] public string RegisterSuccess { get; set; }
        private readonly IUserApplication _userApplication;

        public ActivateUserModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public void OnGet(string id)
        {
            RegisterSuccess = "";
            RegisterMessage = "";
            var result = _userApplication.ActivateUser(id);
            if (result.IsSucceeded)
            {
                RegisterSuccess = ApplicationMessage.SuccessfulActivation;
                // return RedirectToAction("./Authenticate/ActivateUser");
            }
            else
            {
                RegisterMessage = result.Message;
            }
            // return RedirectToAction("./Authenticate/ActivateUser");
        }
    }
}
