﻿using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class EditModel : PageModel
    {
        public List<NotificationViewModel> Notifications;
        public EditListing Command;
        public SelectList CurrencyList;
        private readonly IUserApplication _userApplication;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IListingApplication _listingApplication;
        public EditModel(IUserApplication userApplication,
            IAutenticateHelper autenticateHelper,
            IListingApplication listingApplication
        )
        {
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
            _listingApplication = listingApplication;
        }

        public IActionResult OnGet(long Id)
        {
            var loggedInUserId = _autenticateHelper.CurrentAccountRole().Id;
            Command = _listingApplication.GetEditListing(Id);
            if (Command.OwnerUserId == loggedInUserId)
            {
                CurrencyList = new SelectList(GenerateCurrencyList.GetList());
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public JsonResult OnPost(EditListing command)
        {
            return new JsonResult(_listingApplication.Edit(command));
        }

    }
}
