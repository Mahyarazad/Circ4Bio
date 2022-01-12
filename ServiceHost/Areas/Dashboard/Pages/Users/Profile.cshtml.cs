using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Users
{
    public class ProfileModel : PageModel
    {
        public EditUser user;
        public SelectList CountrlyList;
        public string Role;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProfileModel(IUserApplication userApplication,
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
            Role = _userApplication.GetUsertypes().FirstOrDefault(x => x.TypeId == user.RoleId).TypeName;
        }

        public JsonResult OnPost(EditUser user)
        {
            var result = _userApplication.EditByUser(user);

            return new JsonResult(result);
        }

    }
}
