using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
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

        public IActionResult OnGet(string id)
        {
            var result = _userApplication.ActivateUser(id);
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
