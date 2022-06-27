using System;
using System.Linq;
using System.Threading.Tasks;
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

        [TempData] public string SuccessMessage { get; set; }
        [TempData] public string FailureMessage { get; set; }
        [TempData] public string ActivationFailureMessage { get; set; }

        public void OnGet()
        {
            TempData.Remove("SuccessMessage");
            if (!string.IsNullOrWhiteSpace(_httpContextAccessor.HttpContext.Request.Cookies["user-token"]))
            {
                UserToken = new JavaScriptSerializer()
                    .Deserialize<RememberMe>(
                        _httpContextAccessor.HttpContext.Request.Cookies["user-token"]);
            }
        }

        public IActionResult OnGetLogout()
        {
            TempData.Remove("SuccessMessage");
            _userApplication.Logout();
            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostLogin(EditUser command)
        {

            var result = await _userApplication.Login(command);

            var uri = new Uri(_httpContextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value);
            var queryDictionary = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);

            if (result.IsSucceeded)
            {
                if (queryDictionary.Count > 0)
                    return Redirect(queryDictionary.First().Value);
                TempData["SuccessMessage"] = ApplicationMessage.SuccessLogin;
                return RedirectToPage("/Index", new { area = "Dashboard" });
            }

            if (result.Message == ApplicationMessage.UserNotActive)
            {
                TempData["ActivationFailureMessage"] = result.Message;
            }
            else
            {
                TempData["FailureMessage"] = result.Message;
            }
            return RedirectToPage("/Authentication/Login");
        }

    }
}
