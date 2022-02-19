﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.NotificationAggregate;
using AM.Domain.UserAggregate;
using DealViewModel = AM.Application.Contracts.Deal.DealViewModel;

namespace AM.Application
{
    public class DealApplication : IDealApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IDealRepository _dealRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserApplication _userApplication;
        private readonly IListingRepository _listingRepository;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IEmailService<EmailModel> _emailService;
        private readonly INegotiateRepository _negotiateRepository;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INotificationApplication _notificationApplication;

        public DealApplication(IDealRepository dealRepository, IUserRepository userRepository
            , IListingApplication listingApplication, IEmailService<EmailModel> emailService
            , IFileUploader fileUploader, IUserApplication userApplication
            , IAutenticateHelper autenticateHelper, INegotiateRepository negotiateRepository
            , IListingRepository listingRepository
            , IRecipientRepository recipientRepository, INotificationApplication notificationApplication)
        {
            _fileUploader = fileUploader;
            _emailService = emailService;
            _dealRepository = dealRepository;
            _userRepository = userRepository;
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
            _listingRepository = listingRepository;
            _recipientRepository = recipientRepository;
            _negotiateRepository = negotiateRepository;
            _notificationApplication = notificationApplication;
        }

        public async Task<OperationResult> CreateDeal(CreateDeal Command)
        {
            var result = new OperationResult();


            var negotiate = await _negotiateRepository.Get(Command.NegotiateId);

            var sellerRoleId = _userRepository.GetDetail(negotiate.SellerId).Result.RoleId;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyyerRoleId = _userRepository.GetDetail(negotiate.BuyerId).Result.RoleId;
            var buyyerUserId = $"{_userRepository.GetDetail(negotiate.BuyerId).Result.UserId}";
            var SellerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";
            var buyyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId);

            var buyyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyyerRoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = sellerRoleId
            });

            var buyyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    NotificationBody = $"{SellerUserId} send a quatation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    NotificationBody = $"You have issued a new quatation for {listingInfo.Result.Name} with {buyyerUserId}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.SellerId
                });

            _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyyerRoleId, buyyerNotificationId.Result));
            _recipientRepository.SaveChanges();



            var trackingCode = CodeGenerator.Generate("D#");
            var filePathString = _fileUploader
                .Uploader(Command.ContractFile, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            var deal = new Deal(Command.DeliveryCost, Command.DeliveryMethod, Command.TotalCost, listingInfo.Result.Unit
                , Command.Currency, Command.Amount, Command.Location, trackingCode,
                filePathString, Command.DueTime, Command.ListingId, Command.NegotiateId, negotiate.BuyerId
                , negotiate.SellerId);
            await _negotiateRepository.ActiveNegotiation(Command.NegotiateId);
            _dealRepository.Create(deal);
            _dealRepository.SaveChanges();

            return result.Succeeded();
        }

        public Task<OperationResult> EditDeal(EditDeal Command)
        {
            var result = new OperationResult();
            return Task.FromResult(result);
        }

        public Task<OperationResult> RejectDeal(long Id)
        {
            var result = new OperationResult();
            return Task.FromResult(result);
        }

        public Task<OperationResult> FinishDeal(long Id)
        {
            var result = new OperationResult();
            return Task.FromResult(result);
        }

        public Task<OperationResult> AtivateDeal(long Id)
        {
            var result = new OperationResult();
            return Task.FromResult(result);
        }

        public Task<OperationResult> PaymentReceived(long Id)
        {
            var result = new OperationResult();
            return Task.FromResult(result);
        }

        public Task<List<DealViewModel>> GetAllDeals(long UserId)
        {
            return _dealRepository.GetAllDeals(UserId);
        }

        public Task<DealViewModel> GetDealWithDealId(long DealId)
        {
            return _dealRepository.GetDealWithDealId(DealId);
        }
    }
}