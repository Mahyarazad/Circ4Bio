using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Nace;
using AM.Infrastructure.Core;
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

        [NeedsPermission(UserPermission.GetNace)]
        public IActionResult OnGet(long Id)
        {
            NaceViewModel = _naceApplication.GetSingleNace(Id).Result;
            return null;
        }
    }

}
