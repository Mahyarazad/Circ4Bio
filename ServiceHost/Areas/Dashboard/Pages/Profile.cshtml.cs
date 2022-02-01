﻿using System;
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
        public ProfileModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public void OnGet(string Id)
        {
            if (Int64.TryParse(Id, out long value))
            {
                user = _userApplication.GetDetail(Convert.ToInt64(value));
                CountryList = new SelectList(GenerateCountryList.GetList());
            }
            else
            {
                user = _userApplication.GetDetailByUsername(Id);
                CountryList = new SelectList(GenerateCountryList.GetList());
            }
        }


        public JsonResult OnPost(EditUser user)
        {
            var result = _userApplication.EditByUser(user);
            return new JsonResult(result);
        }
    }
}
