﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class Canceled : PageModel
    {
        public async Task OnGet([FromQuery(Name = "token")] string token)
        {

            
        }
    }
}