using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using AM.Infrastructure.Core;
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

        [RequirePermission(UserPermission.GetNace)]
        public IActionResult OnGet()
        {
            NaceList = _naceApplication.GetAllNaces().Result;
            return null;
        }

        [RequirePermission(UserPermission.GetNace)]
        public IActionResult OnPostGetDeletedNaces(bool isDeleted)
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

        [RequirePermission(UserPermission.DeleteNace)]
        public IActionResult OnGetDeleteNace(long id)
        {
            _naceApplication.DeleteNace(id);
            return RedirectToPage("/Nace/Index", new { area = "Dashboard" });
        }

        [RequirePermission(UserPermission.DeleteNace)]
        public IActionResult OnGetUnDeleteNace(long id)
        {
            _naceApplication.UndeleteNace(id);
            return RedirectToPage("/Nace/Index", new { area = "Dashboard" });
        }
    }
}
