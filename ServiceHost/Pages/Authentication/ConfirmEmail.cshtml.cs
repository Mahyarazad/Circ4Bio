using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework;
using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace ServiceHost.Pages.Authentication
{
    public class ConfirmEmailIndex : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserApplication _userApplication;
        private readonly IResetPasswordApplication _resetPasswordApplication;
        public ResetPasswordModel Command;
        public ConfirmEmailIndex(IHttpContextAccessor httpContextAccessor,
            IUserApplication userApplication,
            IResetPasswordApplication resetPasswordApplication
            )
        {
            _httpContextAccessor = httpContextAccessor;
            _userApplication = userApplication;
            _resetPasswordApplication = resetPasswordApplication;

        }

        [TempData] public string Message { get; set; }
        [TempData] public string SuccessMessage { get; set; }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost(ResetPasswordModel command)
        {
            Message = "";
            SuccessMessage = "";

            var result = await _resetPasswordApplication.CreateResetPassword(command.Email);
            if (result.IsSucceeded)
            {

                SuccessMessage = ApplicationMessage.ResetPasswordGuidance;
                return RedirectToPage("/Authentication/ConfirmEmail", new { area = "" });
            }
            Message = result.Message;
            return RedirectToPage("/Authentication/ConfirmEmail", new { area = "" });
        }
    }
}
