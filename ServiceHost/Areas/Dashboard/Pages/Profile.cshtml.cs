using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class ProfileModel : PageModel
    {
        public EditUser user;
        public SelectList CountryList;
        public string Role;
        private readonly IUserApplication _userApplication;
        public ProfileModel(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        public void OnGet(long Id)
        {
            user = _userApplication.GetDetail(Id);
            CountryList = new SelectList(GenerateCountryList.GetList());
        }

        public JsonResult OnPost(EditUser user)
        {
            var result = _userApplication.EditByUser(user);
            return new JsonResult(result);
        }
    }
}
