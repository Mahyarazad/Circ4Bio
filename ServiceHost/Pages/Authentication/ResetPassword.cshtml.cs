using System;
using System.Collections.Generic;
using _0_Framework;
using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;

namespace ServiceHost.Pages.Authentication
{
    public class ResetPasswordIndex : PageModel
    {
        private readonly IUserApplication _userApplication;
        private readonly IResetPasswordApplication _resetPasswordApplication;
        public ResetPasswordModel Command;
        public ResetPasswordIndex(IResetPasswordApplication resetPasswordApplication,
            IUserApplication userApplication)
        {
            _userApplication = userApplication;
            _resetPasswordApplication = resetPasswordApplication;
        }

        [TempData] public string Message { get; set; }
        [TempData] public string SuccessMessage { get; set; }
        public long UserId { get; set; }
        public void OnGet(string guid)
        {
            TempData.Clear();
            Command = _resetPasswordApplication.GetResetPasswordGuid(guid).Result;
            if (Command.Guid == new Guid())
            {
                Message = ApplicationMessage.ResetPasswordLinkIsInvalid;
                return;
            }
            if (!Command.IsValid)
                Message = ApplicationMessage.ResetPasswordLinkExipre;
        }


        public IActionResult OnPostUpdate(ResetPasswordModel command)
        {
            TempData.Clear();
            var guid = HttpContext.Request.Path.Value.ToString().Split("/");
            command.UserId = _resetPasswordApplication.GetResetPasswordGuid(guid[3]).Result.UserId;
            var result = _userApplication.ResetPassword(command).Result;

            if (result.IsSucceeded)
            {
                SuccessMessage = result.Message;
                return RedirectToPage("/Authentication/Login");
            }
            else
            {
                return RedirectToPage($"/Authentication/ResetPassword/{guid}");
            }

        }

    }
}
