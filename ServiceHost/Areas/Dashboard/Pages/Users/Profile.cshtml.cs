using System.Linq;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Users
{
    public class ProfileModel : PageModel
    {
        public EditUser user;

        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        public ProfileModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor
            )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
        }

        public void OnGet()
        {
            var userId = _contextAccessor.HttpContext.User.Claims.ToList().FirstOrDefault(x => x.Type == "User Id").Value;
            user = _userApplication.GetDetail(long.Parse(userId));
        }

    }
}
