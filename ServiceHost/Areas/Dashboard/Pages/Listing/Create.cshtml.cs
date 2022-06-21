using System.Collections.Generic;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.NaceData;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Domain.NaceAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ServiceHost.Areas.Dashboard.Pages.Listing
{
    public class CreateModel : PageModel
    {
        public CreateListing Command;
        public SelectList CurrencyList;
        public SelectList DeliveryCharges;
        public SelectList NaceSelectList;
        public List<NaceViewModel> NaceViewModelList;
        public NaceDataDTO NaceData;

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

        public void OnGet()
        {
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

        }

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
