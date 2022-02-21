﻿using System.Collections.Generic;
using AM.Application.Contracts.ContactUs;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.ContactUs
{
    public class IndexModel : PageModel
    {
        public EditUser user;

        private readonly IContactUsApplication _contactUsApplication;
        public ContactUsViewModel Command;
        public List<ContactUsViewModel> ContactUsMessagList;
        [TempData]
        public string Message { get; set; }

        public IndexModel(IContactUsApplication contactUsApplication)
        {
            _contactUsApplication = contactUsApplication;
        }

        public async void OnGet()
        {
            ContactUsMessagList = await _contactUsApplication.GetContactUsMessages();
        }

        public JsonResult OnPostIsReed(long id)
        {
            return new JsonResult(_contactUsApplication.MarkAsRead(id));
        }
        public async void OnPostGetAll(ContactUsViewModel Command)
        {
            ContactUsMessagList = await _contactUsApplication.GetAllContactUsMessages();
        }
        public async void OnPostGetReadMessages(ContactUsViewModel Command)
        {
            ContactUsMessagList = await _contactUsApplication.GetReadContactUsMessages();
        }
    }
}
