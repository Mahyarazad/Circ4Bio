﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public EditListing Command;
        public SelectList CurrencyList;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        public EditModel(
            IAuthenticateHelper authenticateHelper,
            IListingApplication listingApplication
        )
        {
            _authenticateHelper = authenticateHelper;
            _listingApplication = listingApplication;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            var loggedInUserId = _authenticateHelper.CurrentAccountRole().Id;
            Command = await _listingApplication.GetEditListing(Id);
            if (Command.OwnerUserId == loggedInUserId | loggedInUserId == 1)
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
