using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNet.SignalR;
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

        [NeedsPermission(UserPermission.EditUser)]
        public async void OnGet(long Id)
        {
            user = await _userApplication.GetDetail(Id);
            CountrlyList = new SelectList(GenerateCountryList.GetList());
            RoleList = new SelectList(await _userApplication.GetAllUsertypes(), "TypeId", "TypeName");
        }

        [NeedsPermission(UserPermission.EditUser)]
        public JsonResult OnPost(EditUser user)
        {
            return new JsonResult(_userApplication.EditByAdmin(user));
        }

    }
}
