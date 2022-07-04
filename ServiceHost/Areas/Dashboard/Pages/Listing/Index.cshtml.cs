using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class IndexModel : PageModel
    {
        public EditUser user;
        public List<ListingViewModel> Listing;
        public bool IsFiltered { get; set; }
        public bool MasterFilter;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingApplication _listingApplication;

        public IndexModel(IUserApplication userApplication,
            IHttpContextAccessor contextAccessor,
            IListingApplication listingApplication
        )
        {
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingApplication = listingApplication;
        }
        [RequirePermission(UserPermission.GetListing)]
        public async Task OnGet()
        {
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = await _userApplication.GetDetail(userId);

            var AdminCheck = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (AdminCheck == "1")
            {
                Listing = await _listingApplication.GetAllListingForAdmin();
                Listing = Listing.Where(x => !x.IsDeleted).ToList();
            }
            else
            {
                Listing = await _listingApplication.GetUserListing(user.Id);
            }

        }
        [RequirePermission(UserPermission.GetListing)]
        public async Task OnPostShowDeleted(bool IsFiltered)
        {
            var userId = long.Parse(_contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == "User Id").Value);

            user = await _userApplication.GetDetail(userId);
            var AdminCheck = _contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (IsFiltered)
            {
                MasterFilter = true;
                if (AdminCheck == "1")
                {
                    Listing = await _listingApplication.GetAllListingForAdmin();
                }
                else
                {
                    Listing = await _listingApplication.GetDeletedUserListing(userId);
                }
            }
            else
            {
                MasterFilter = false;
                if (AdminCheck == "1")
                {
                    Listing = await _listingApplication.GetAllListing();
                    Listing = Listing.Where(x => !x.IsDeleted).ToList();
                }
                else
                {
                    Listing = await _listingApplication.GetUserListing(user.Id);
                }
            }

        }

        [RequirePermission(UserPermission.DeleteListing)]
        public Task<JsonResult> OnPostMarkDelete(long id)
        {
            return Task.FromResult(new JsonResult(_listingApplication.Delete(id)));
        }

        public Task<JsonResult> OnPostMarkPublic(long id)
        {
            return Task.FromResult(new JsonResult(_listingApplication.MarkPublic(id)));
        }

        public Task<JsonResult> OnPostMarkPrivate(long id)
        {
            return Task.FromResult(new JsonResult(_listingApplication.MarkPrivate(id)));
        }

        public IActionResult OnGetIncrement(long id)
        {
            var inputAmount = new InputAmount()
            {
                ListingId = id
            };
            return Partial("./Increment", inputAmount);
        }

        public IActionResult OnGetDecrement(long id)
        {
            var inputAmount = new InputAmount()
            {
                ListingId = id
            };
            return Partial("./Decrement", inputAmount);
        }

        public Task<JsonResult> OnPostIncrement(InputAmount command)
        {
            return Task.FromResult(new JsonResult(_listingApplication.IncrementAmount(command)));
        }
        public Task<JsonResult> OnPostDecrement(InputAmount command)
        {
            return Task.FromResult(new JsonResult(_listingApplication.DecrementAmount(command)));
        }
        public async Task<IActionResult> OnGetLog(long id)
        {
            return Partial("./Log", await _listingApplication.GetListingOperationLog(id));
        }
    }
}
