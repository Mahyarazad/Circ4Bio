using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Blog;
using AM.Application.Contracts.Nace;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Nace
{
    public class IndexModel : PageModel
    {

        private readonly INaceApplication _naceApplication;

        public IndexModel(INaceApplication naceApplication)
        {
            _naceApplication = naceApplication;
        }

        public void OnGet()
        {

        }

    }
}
