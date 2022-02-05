using _0_Framework.Application;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Users.Account
{
    public class EditModel : PageModel
    {
        public EditUser user;
        public SelectList CountrlyList;
        public SelectList RoleList;
        private readonly IUserApplication _userApplication;
        public EditModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }
        [RequirePermission(UserPermission.EditUser)]
        public void OnGet(long Id)
        {
            user = _userApplication.GetDetail(Id);
            CountrlyList = new SelectList(GenerateCountryList.GetList());
            RoleList = new SelectList(_userApplication.GetUsertypes(), "TypeId", "TypeName");
        }

        [RequirePermission(UserPermission.EditUser)]
        public JsonResult OnPost(EditUser user)
        {
            return new JsonResult(_userApplication.EditByAdmin(user));
        }

    }
}
