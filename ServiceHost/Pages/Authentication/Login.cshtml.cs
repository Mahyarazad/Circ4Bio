using System;
using System.Collections.Generic;
using _0_Framework;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace ServiceHost.Pages.Authentication
{
    public class LoginIndex : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserApplication _userApplication;
        public EditUser Command;
        public RememberMe UserToken { get; set; }
        public LoginIndex(IHttpContextAccessor httpContextAccessor,
            IUserApplication userApplication)
        {
            _httpContextAccessor = httpContextAccessor;
            _userApplication = userApplication;
        }

        [TempData] public string LoginMessage { get; set; }

        public void OnGet()
        {
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.Request.Cookies["user-token"]))
            {
                UserToken = new JavaScriptSerializer()
                    .Deserialize<RememberMe>(
                        _httpContextAccessor.HttpContext.Request.Cookies["user-token"]);
            }
        }

        public IActionResult OnGetLogout()
        {
            LoginMessage = "";
            _userApplication.Logout();
            return RedirectToPage("./Index");
        }

        public IActionResult OnPostLogin(EditUser command)
        {
            LoginMessage = "";

            var result = _userApplication.Login(command);
            if (result.IsSucceeded)
            {
                LoginMessage = ApplicationMessage.SuccessLogin;
                return RedirectToPage("/Index", new { area = "Dashboard" });
            }
            LoginMessage = result.Message;
            return RedirectToAction("/Authentication/Login");
        }

    }
}