using System.Collections.Generic;
using System.Linq;
using System.Timers;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Users.Account
{
    public class IndexModel : PageModel
    {
        public EditUser user;

        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        public UserSearchModel Command;
        public List<UserViewModel> UserList;
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string Message { get; set; }

        public IndexModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor
            )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
        }

        public void OnGet(UserSearchModel Command)
        {
            UserList = _userApplication.Search(Command);
            var userId = _contextAccessor.HttpContext.User.Claims.ToList().FirstOrDefault(x => x.Type == "User Id").Value;
            user = _userApplication.GetDetail(long.Parse(userId));
        }

        public JsonResult OnPostActivateUser(long id)
        {
            var result = _userApplication.AdminActivateUser(id);
            return new JsonResult(result);
        }
        public JsonResult OnPostDeactivateUser(long id)
        {
            var result = _userApplication.AdminDectivateUser(id);
            return new JsonResult(result);
        }
    }
}
