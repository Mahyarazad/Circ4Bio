using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.NaceData;
using AM.Application.Contracts.User;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class CreateModel : PageModel
    {
        public CreateListing Command;
        public AuthViewModel LoggedUser;
        public SelectList CurrencyList;
        public SelectList DeliveryCharges;
        public SelectList NaceSelectList;
        public List<NaceViewModel> NaceViewModelList;
        public NaceDataDTO NaceData;
        public SelectList DeliveryLocationSelectList;

        private readonly IUserApplication _userApplication;
        private readonly INaceApplication _naceApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INaceDataApplication _naceDataApplication;


        private readonly IListingApplication _listingApplication;

        public CreateModel(IUserApplication userApplication, INaceApplication naceApplication, IAuthenticateHelper authenticateHelper, INaceDataApplication naceDataApplication, IListingApplication listingApplication)
        {
            _userApplication = userApplication;
            _naceApplication = naceApplication;
            _authenticateHelper = authenticateHelper;
            _naceDataApplication = naceDataApplication;
            _listingApplication = listingApplication;
        }
        [RequirePermission(UserPermission.CreateListing)]
        public void OnGet()
        {
            LoggedUser = _authenticateHelper.CurrentAccountRole();
            Command = new CreateListing();
            CurrencyList = new SelectList(GenerateCurrencyList.GetList());
            DeliveryCharges = new SelectList(new List<string>
            {
                new("Buyer"),
                new ("Seller"),
            });
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
            var listView =
                _userApplication
                    .GetDeliveryLocationDropDown(_authenticateHelper.CurrentAccountRole().Id).Result;
            DeliveryLocationSelectList = new SelectList(listView, "LocationId", "Name");
        }
        [RequirePermission(UserPermission.CreateListing)]
        public JsonResult OnPost(CreateListing command, NaceDataDTO naceData)
        {
            var result = _listingApplication.Create(command);
            naceData.ListingId = _listingApplication.LastCreatedListingId();
            if (naceData.NaceId != 0)
            {
                _naceDataApplication.CreateNaceData(naceData);
            }
            return new JsonResult(result);
        }

    }
}
