using System;
using System.Threading.Tasks;
using _0_Framework.Application;
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
        private readonly IAuthenticateHelper _authenticateHelper;
        public ProfileModel(IUserApplication userApplication,
            IAuthenticateHelper authenticateHelper)
        {
            _userApplication = userApplication;
            _authenticateHelper = authenticateHelper;
        }

        public async Task<IActionResult> OnGet(string Id)
        {
            CountryList = new SelectList(GenerateCountryList.GetList());
            if (Int64.TryParse(Id, out long value))
            {
                user = await _userApplication.GetDetail(Convert.ToInt64(value));
                if (user.Id == _authenticateHelper.CurrentAccountRole().Id)
                {
                    return null;
                }
                else
                {
                    return RedirectToPage("AccessDenied", new { area = "" });
                }
            }
            else
            {
                user = await _userApplication.GetDetailByUsername(Id);
                if (user.Id == _authenticateHelper.CurrentAccountRole().Id)
                {
                    return null;
                }
                else
                {
                    return RedirectToPage("AccessDenied", new { area = "" });
                }

            }
        }

        public async Task<JsonResult> OnPost(EditUser user)
        {
            var result = await _userApplication.EditByUser(user);
            return new JsonResult(Task.FromResult(result));
        }
        public IActionResult OnGetAddDeliveryLocation(long id)
        {
            var user = new CreateDeliveryLocation
            {
                UserId = id
            };
            return Partial("./AddDeliveryLocation", user);
        }
        public async Task<IActionResult> OnGetListDeliveryLocation(long id)
        {
            return Partial("./ListDeliveryLocation", await _userApplication.GetDeliveryLocationList(id));
        }
        public async Task<JsonResult> OnPostAddDeliveryLocation(CreateDeliveryLocation Command)
        {
            var result = await _userApplication.AddDeliveryLocation(Command);
            return new JsonResult(Task.FromResult(result));
        }

        public async Task<IActionResult> OnGetEditDeliveryLocation(long userId, long locationId)
        {
            return Partial("./EditDeliveryLocation", await _userApplication.GetDeliveryLocation(userId, locationId));
        }
        public async Task<JsonResult> OnPostEditDeliveryLocation(CreateDeliveryLocation Command)
        {
            var result = await _userApplication.EditDeliveryLocation(Command);
            return new JsonResult(Task.FromResult(result));
        }
    }
}
