﻿using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Dashboard.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDealApplication _dealApplication;
        private readonly IUserApplication _userApplication;
        private readonly IListingApplication _listingApplication;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;

        public IndexModel(IDealApplication dealApplication, IListingApplication listingApplication
            , IAuthenticateHelper authenticateHelper, INegotiateApplication negotiateApplication,
            IUserApplication userApplication)
        {
            _dealApplication = dealApplication;
            _userApplication = userApplication;
            _listingApplication = listingApplication;
            _authenticateHelper = authenticateHelper;
            _negotiateApplication = negotiateApplication;
        }

        public long ActiveListingCount { get; set; }
        public long ActiveNegotiationsSalesItems { get; set; }
        public long ActiveNegotiationsBuyingItems { get; set; }
        public long UserId { get; set; }
        public long SuppliedItems { get; set; }
        public long PurchasedItems { get; set; }
        public long TotalUsers { get; set; }
        public long CanceledNegotation { get; set; }
        public long FinishedNegotation { get; set; }
        public long QuatationSentButNotFinishedNegotation { get; set; }
        public long ActiveNegotations { get; set; }
        public long AllNegotations { get; set; }
        public long StatusFalseTotalUsers { get; set; }
        public long PaymentsDone { get; set; }
        public long PaymentsPending { get; set; }
        public List<UserStat> UserStat = new List<UserStat>();
        public List<CreateNegotiate> NegotationStat { get; set; }
        public List<ListingViewModel> ListingStat { get; set; }
        public void OnGet()
        {

            UserId = _authenticateHelper.CurrentAccountRole().Id;


            if (UserId == 1)
            {
                ListingStat = _listingApplication.GetAllPublicListing().Result;
                TotalUsers = _userApplication.GetFullList().Result.Count;
                StatusFalseTotalUsers = _userApplication.GetFullList().Result.Where(x => !x.Status).Count();
                NegotationStat = _negotiateApplication.GetAllListingNegotiation();
                QuatationSentButNotFinishedNegotation =
                    NegotationStat.Where(x => x.QuatationSent && !x.IsFinished).Count();
                FinishedNegotation = NegotationStat.Where(x => x.IsFinished).Count();
                CanceledNegotation = NegotationStat.Where(x => x.IsCanceled).Count();
                ActiveNegotations = NegotationStat.Where(x => !x.QuatationSent && !x.IsFinished && !x.IsCanceled).Count();
                AllNegotations = NegotationStat.Count;

                PaymentsDone = _dealApplication.GetAllDeals().Result.Where(x => x.PaymentId != null).Count();
                PaymentsPending = _dealApplication.GetAllDeals().Result.Where(x => x.PaymentId == null).Count();


                foreach (var type in _userApplication.GetUsertypes().Result)
                {
                    UserStat.Add(new UserStat
                    {
                        Id = type.TypeId,
                        RoleType = type.TypeName
                    });
                }

                foreach (var User in _userApplication.GetFullList().Result)
                {
                    switch (User.RoleId)
                    {
                        case 2:
                            UserStat.FirstOrDefault(x => x.Id == 2).Count++;
                            break;
                        case 3:
                            UserStat.FirstOrDefault(x => x.Id == 3).Count++;
                            break;

                        case 4:
                            UserStat.FirstOrDefault(x => x.Id == 4).Count++;
                            break;
                        case 5:
                            UserStat.FirstOrDefault(x => x.Id == 5).Count++;
                            break;
                    }
                }

            }
            else
            {
                ActiveListingCount = _listingApplication.GetActiveListing(UserId).Result.Count;
                ActiveNegotiationsSalesItems =
                    _negotiateApplication
                        .AllListingItemsBuyyer(UserId)
                        .Where(x => !x.IsFinished & !x.IsCanceled).Count();
                ActiveNegotiationsBuyingItems =
                    _negotiateApplication
                        .AllListingItemsSeller(UserId)
                         .Where(x => !x.IsFinished & !x.IsCanceled).Count();
                SuppliedItems = _dealApplication.GetSuppliedList(UserId).Result.Count;
                PurchasedItems = _dealApplication.GetPurchasedList(UserId).Result.Count;
            }
        }
    }
}