using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using AM.Infrastructure.Core;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Nace
{
    public class ViewTableModel : PageModel
    {

        private readonly INaceApplication _naceApplication;
        private readonly IAuthenticateHelper _authenticateHelper;

        public ViewTableModel(INaceApplication naceApplication
            , IAuthenticateHelper authenticateHelper)
        {
            _naceApplication = naceApplication;
            _authenticateHelper = authenticateHelper;
        }

        public NaceViewModel NaceViewModel { get; set; }
        public int Counter { get; set; }

        [RequirePermission(UserPermission.GetNace)]
        public IActionResult OnGet(long Id)
        {
            if (_authenticateHelper.CurrentAccountRole().Id == 1)
            {
                NaceViewModel = _naceApplication.GetSingleNace(Id).Result;
                return null;
            }

            return RedirectToPage("/AccessDenied", new { area = "" });
        }
    }

}
