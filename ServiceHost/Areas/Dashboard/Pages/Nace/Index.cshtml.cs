using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Nace
{
    public class IndexModel : PageModel
    {

        private readonly INaceApplication _naceApplication;
        private readonly IAuthenticateHelper _authenticateHelper;

        public IndexModel(INaceApplication naceApplication
            , IAuthenticateHelper authenticateHelper)
        {
            _naceApplication = naceApplication;
            _authenticateHelper = authenticateHelper;
        }

        public List<NaceViewModel> NaceList { get; set; }
        public bool IsDeleted { get; set; }
        public int Counter { get; set; }
        [Authorize]
        public IActionResult OnGet()
        {
            if (_authenticateHelper.CurrentAccountRole().Id == 1)
            {
                NaceList = _naceApplication.GetAllNaces().Result;
                return null;
            }
            return RedirectToPage("/AccessDenied", new { area = "" });
        }

        public IActionResult OnPostGetDeletedNaces(bool isDeleted)
        {
            if (_authenticateHelper.CurrentAccountRole().Id == 1)
            {
                IsDeleted = isDeleted;
                if (IsDeleted)
                {
                    NaceList = _naceApplication
                        .GetAllNaces().Result
                        .Where(x => x.IsDeleted).ToList();

                    return null;
                }
                return RedirectToPage("/Nace/Index", new { area = "Dashboard" });

            }
            return RedirectToPage("/AccessDenied", new { area = "" });
        }

        public IActionResult OnGetDeleteNace(long id)
        {
            if (_authenticateHelper.CurrentAccountRole().Id == 1)
            {
                _naceApplication.DeleteNace(id);
                return RedirectToPage("/Nace/Index", new { area = "Dashboard" });
            }
            return RedirectToPage("/AccessDenied", new { area = "" });
        }

        public IActionResult OnGetUnDeleteNace(long id)
        {
            if (_authenticateHelper.CurrentAccountRole().Id == 1)
            {
                _naceApplication.UndeleteNace(id);
                return RedirectToPage("/Nace/Index", new { area = "Dashboard" });
            }
            return RedirectToPage("/AccessDenied", new { area = "" });
        }
    }
}
