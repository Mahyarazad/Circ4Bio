using System;
using System.Globalization;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class ProfileModel : PageModel
    {
        public EditUser user;
        public SelectList CountryList;
        public string Role;
        private readonly IUserApplication _userApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        public ProfileModel(IUserApplication userApplication,
            IAutenticateHelper autenticateHelper)
        {
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
        }

        public IActionResult OnGet(string Id)
        {
            CountryList = new SelectList(GenerateCountryList.GetList());
            if (Int64.TryParse(Id, out long value))
            {
                user = _userApplication.GetDetail(Convert.ToInt64(value));
                if (user.Id == _autenticateHelper.CurrentAccountRole().Id)
                {
                    return null;
                }
                else
                {
                    return RedirectToPage("AccessDenied", new { area = "" });
                }
            }
            else
            {
                user = _userApplication.GetDetailByUsername(Id);
                if (user.Id == _autenticateHelper.CurrentAccountRole().Id)
                {
                    return null;
                }
                else
                {
                    return RedirectToPage("AccessDenied", new { area = "" });
                }

            }
        }


        public JsonResult OnPost(EditUser user)
        {
            var result = _userApplication.EditByUser(user);
            return new JsonResult(result);
        }
    }
}
