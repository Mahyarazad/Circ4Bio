using System;
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
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IEmailService<EmailModel> _emailService;
        private readonly INegotiateRepository _negotiateRepository;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INotificationApplication _notificationApplication;

        public DealApplication(IDealRepository dealRepository, IUserRepository userRepository
            , IListingApplication listingApplication, IEmailService<EmailModel> emailService
            , IFileUploader fileUploader, IUserApplication userApplication
            , IAuthenticateHelper authenticateHelper, INegotiateRepository negotiateRepository
            , IListingRepository listingRepository, IHttpContextAccessor contextAccessor
            , IRecipientRepository recipientRepository, INotificationApplication notificationApplication)
        {
            _fileUploader = fileUploader;
            _emailService = emailService;
            _dealRepository = dealRepository;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _userApplication = userApplication;
            _listingRepository = listingRepository;
            _authenticateHelper = authenticateHelper;
            _recipientRepository = recipientRepository;
            _negotiateRepository = negotiateRepository;
            _notificationApplication = notificationApplication;
        }

        public async Task<OperationResult> CreateQuatation(CreateDeal Command)
        {
            var result = new OperationResult();

            var RedirectUrlSeller = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value.ToString().Replace("create", "quatation");
            var RedirectUrlBuyer = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value.ToString().Replace("create", "confirmquatation");


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
                    RedirectUrl = RedirectUrlBuyer,
                    NotificationBody = $"{SellerUserId} send a quatation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = RedirectUrlSeller,
                    NotificationBody = $"You have issued a new quatation for {listingInfo.Result.Name} with {buyyerUserId}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.SellerId
                });

            _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyyerRoleId, buyyerNotificationId.Result));
            _recipientRepository.SaveChanges();



            var trackingCode = "Draft Quatation";
            var filePathString = _fileUploader
                .Uploader(Command.ContractFile, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            var deal = new Deal(Command.DeliveryCost, Command.DeliveryMethod, Command.TotalCost, listingInfo.Result.Unit
                , Command.Currency, Command.Amount, Convert.ToInt32(Command.Location), trackingCode,
                filePathString, Command.DueTime, Command.ListingId, Command.NegotiateId, negotiate.BuyerId
                , negotiate.SellerId);

            deal.QuatationHasSent();
            negotiate.QuatationHasSent();
            await _negotiateRepository.ActiveNegotiation(Command.NegotiateId);
            _dealRepository.Create(deal);
            _negotiateRepository.SaveChanges();
            _dealRepository.SaveChanges();

            return result.Succeeded();
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
                , Command.Currency, Command.Amount, Convert.ToInt32(Command.Location), trackingCode,
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
            if (!_dealRepository.Exist(x => x.Id == Command.DealId))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            var quatation = _dealRepository.Get(Command.DealId).Result;

            var filePathString = _fileUploader
                .Uploader(Command.ContractFile, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            var RedirectUrlSeller = _contextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;
            var RedirectUrlBuyer = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value.ToString().Replace("quatation", "confirmquatation");

            ////Notification Push
            var negotiate = _negotiateRepository.Get(Command.NegotiateId).Result;

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
                    RedirectUrl = RedirectUrlBuyer,
                    NotificationBody = $"{SellerUserId} update the quatation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = RedirectUrlSeller,
                    NotificationBody = $"You have updated the quatation for {listingInfo.Result.Name} with {buyyerUserId}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.SellerId
                });

            _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyyerRoleId, buyyerNotificationId.Result));
            _recipientRepository.SaveChanges();





            quatation.Edit(Command.DeliveryCost, Command.DeliveryMethod, Command.TotalCost, Command.Unit
                , Command.Currency, Command.Amount, Command.Location, filePathString);
            _dealRepository.SaveChanges();

            return Task.FromResult(result.Succeeded());
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

        public DealViewModel GetDealWithDealId(long DealId)
        {
            return  _dealRepository.GetDealWithDealId(DealId);
        }
    }
}