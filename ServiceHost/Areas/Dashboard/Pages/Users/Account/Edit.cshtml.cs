using _0_Framework.Application;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _contextAccessor;
        public EditModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor
            )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
        }

        public void OnGet(long Id)
        {
            user = _userApplication.GetDetail(Id);
            CountrlyList = new SelectList(GenerateCountryList.GetList());
            RoleList = new SelectList(_userApplication.GetUsertypes(), "TypeId", "TypeName");
        }

        public JsonResult OnPost(EditUser user)
        {
            var result = _userApplication.EditByAdmin(user);

            return new JsonResult(result);
        }

    }
}
