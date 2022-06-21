using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.NaceData;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class EditModel : PageModel
    {
        public EditListing Command;
        public SelectList CurrencyList;
        public SelectList NaceSelectList;
        public List<NaceViewModel> NaceViewModelList;
        public NaceDataViewModel NaceData;
        public List<NaceDataDetail> NaceDataDetail;
        public NaceViewModel NaceViewModel;

        public EditModel(INaceApplication naceApplication, IAuthenticateHelper authenticateHelper, IListingApplication listingApplication, INaceDataApplication naceDataApplication)
        {
            _naceApplication = naceApplication;
            _authenticateHelper = authenticateHelper;
            _listingApplication = listingApplication;
            _naceDataApplication = naceDataApplication;
        }

        private readonly INaceApplication _naceApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IListingApplication _listingApplication;
        private readonly INaceDataApplication _naceDataApplication;

        [RequirePermission(UserPermission.EditListing)]
        public async Task<IActionResult> OnGet(long Id)
        {
            var loggedInUserId = _authenticateHelper.CurrentAccountRole().Id;
            Command = await _listingApplication.GetEditListing(Id);

            if (Command.OwnerUserId == loggedInUserId | loggedInUserId == 1)
            {
                NaceData = _naceDataApplication.GetNaceData(Id);
                NaceDataDetail = NaceData.NaceDataDetails;

                NaceViewModel = _naceApplication.GetSingleNace(NaceData.NaceId).Result;
                Command.NaceData = new NaceDataDTO();

                CurrencyList = new SelectList(GenerateCurrencyList.GetList());
                NaceViewModelList = new List<NaceViewModel>();

                _naceApplication.GetAllNaces()
                    .Result.ForEach(x =>
                    {
                        if (!x.IsDeleted)
                            NaceViewModelList.Add(new NaceViewModel
                            {
                                NaceId = x.NaceId,
                                Title = x.Title
                            });
                    });

                NaceSelectList = new SelectList(NaceViewModelList, "NaceId", "Title");
                return null;
            }
            return RedirectToPage("/AccessDenied", new { area = "" });
        }
        [RequirePermission(UserPermission.EditListing)]
        public JsonResult OnPost(EditListing command, NaceDataDTO naceData)
        {
            if (command.NaceData != null)
                _naceDataApplication.EditNaceData(command.NaceData);
            if (naceData.ItemdetailIndex != null)
            {
                naceData.ListingId = command.Id;
                _naceDataApplication.CreateNaceData(naceData);
            }


            return new JsonResult(_listingApplication.Edit(command));
        }
        [RequirePermission(UserPermission.DeleteNaceData)]
        public void OnGetDeleteNace(long Id)
        {
            _naceDataApplication.DeleteNaceData(Id);
        }
    }
}
