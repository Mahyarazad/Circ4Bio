﻿using System.Collections.Generic;
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
    public class CreateModel : PageModel
    {
        public List<NotificationViewModel> Notifications;
        public CreateListing Command;
        public SelectList CurrencyList;

        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly IUserApplication _userApplication;
        public CreateModel(
            IAutenticateHelper autenticateHelper,
            IUserApplication userApplication,
            IListingApplication listingApplication)
        {
            _autenticateHelper = autenticateHelper;
            _userApplication = userApplication;
            _listingApplication = listingApplication;
        }

        public void OnGet()
        {
            Command = new CreateListing();
            CurrencyList = new SelectList(GenerateCurrencyList.GetList());

        }

        public JsonResult OnPost(CreateListing command)
        {
            return new JsonResult(_listingApplication.Create(command));
        }

    }
}
