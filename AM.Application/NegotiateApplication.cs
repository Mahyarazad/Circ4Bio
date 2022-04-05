using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.NotificationAggregate;
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;

namespace AM.Application
{
    public class NegotiateApplication : INegotiateApplication
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IFileUploader _fileUploader;
        private readonly IUserRepository _userRepository;
        private readonly IDealRepository _dealRepository;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingRepository _listingRepository;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IEmailService<EmailModel> _emailService;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INegotiateRepository _negotiateRepository;
        private readonly IUserNegotiateRepository _userNegotiateRepository;
        private readonly INotificationApplication _notificationApplication;
        public NegotiateApplication(INegotiateRepository negotiateRepository,
            IAuthenticateHelper authenticateHelper,
            IMemoryCache memoryCache,
            IRecipientRepository recipientRepository,
            INotificationApplication notificationApplication,
            IHttpContextAccessor contextAccessor,
            IDealRepository dealRepository,
            IEmailService<EmailModel> emailService,
            IUserApplication userApplication,
            IListingRepository listingRepository,
            IUserNegotiateRepository userNegotiateRepository,
            IFileUploader fileUploader,
            IUserRepository userRepository)
        {
            _memoryCache = memoryCache;
            _fileUploader = fileUploader;
            _emailService = emailService;
            _userRepository = userRepository;
            _dealRepository = dealRepository;
            _userApplication = userApplication;
            _contextAccessor = contextAccessor;
            _listingRepository = listingRepository;
            _authenticateHelper = authenticateHelper;
            _recipientRepository = recipientRepository;
            _negotiateRepository = negotiateRepository;
            _userNegotiateRepository = userNegotiateRepository;
            _notificationApplication = notificationApplication;
        }


        public async Task<OperationResult> Create(CreateNegotiate Command)
        {
            var result = new OperationResult();

            var RedirectUrl = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Origin").Value
                .ToString();


            if (!_listingRepository.Exist(x => x.Id == Command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);
            if (_negotiateRepository.Exist(x => x.SellerId == Command.SellerId &
                                                x.BuyerId == Command.BuyerId &
                                                x.ListingId == Command.ListingId & !x.IsFinished))
            {
                if (_negotiateRepository.Exist(x => x.SellerId == Command.SellerId &
                                                    x.BuyerId == Command.BuyerId &
                                                    x.ListingId == Command.ListingId &
                                                    !x.IsCanceled))
                    return result.Failed(ApplicationMessage.DuplicateNegotiation);
            }


            var sellerInfo = _userRepository.GetDetail(Command.SellerId).Result;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerInfo.RoleId).TypeName;

            var buyerInfo = _userRepository.GetDetail(Command.BuyerId).Result;
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyerInfo.RoleId).TypeName;

            var listingInfo = await _listingRepository.GetListingDetail(Command.ListingId);

            var buyyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyyerRecipientList.Add(new RecipientViewModel
            {
                UserId = Command.BuyerId,
                IsReed = false,
                RoleId = buyerInfo.RoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = Command.SellerId,
                IsReed = false,
                RoleId = sellerInfo.RoleId
            });

            var negotiate = new Negotiate(Command.ListingId, Command.BuyerId, Command.SellerId);
            _negotiateRepository.Create(negotiate);
            _negotiateRepository.SaveChanges();

            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyyerRecipientList,
                    SenderId = Command.BuyerId,
                    RedirectUrl = RedirectUrl + $"/dashboard/negotiate/messages/{negotiate.Id}",
                    NotificationBody = $"Negotiation opened for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.SubmitNegotiationRequest,
                    UserId = Command.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = Command.SellerId,
                    RedirectUrl = RedirectUrl + $"/dashboard/negotiate/messages/{negotiate.Id}",
                    NotificationBody = $"{buyerInfo.UserId.ToUpper()} opened a negotiation for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.ReceivedNegotiation,
                    UserId = Command.SellerId
                });

            _recipientRepository.Create(new Recipient(Command.BuyerId, buyerInfo.RoleId, buyerNotificationId.Result));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerInfo.RoleId, sellerNotificationId.Result));
            _recipientRepository.SaveChanges();



            _userNegotiateRepository.Create(new UserNegotiate(Command.BuyerId, negotiate.Id, true));
            _userNegotiateRepository.Create(new UserNegotiate(Command.SellerId, negotiate.Id, false));
            _userNegotiateRepository.Save();


            var emailBuyer = new EmailModel
            {
                EmailTemplate = 3,
                Title = ApplicationMessage.SubmitNegotiationRequest,
                Body3 = RedirectUrl + $"/dashboard/negotiate/messages/{negotiate.Id}",
                Recipient = buyerInfo.Email,
                Body = ApplicationMessage.SubmitNegotiationRequest,
                Body1 = $"Negotiation opened for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                Body4 = "Go to the negotiation"

            };
            var emailSeller = new EmailModel
            {
                EmailTemplate = 3,
                Title = ApplicationMessage.ReceivedNegotiation,
                Body3 = RedirectUrl + $"/dashboard/negotiate/messages/{negotiate.Id}",
                Recipient = sellerInfo.Email,
                Body = ApplicationMessage.ReceivedNegotiation,
                Body1 = $"{buyerInfo.UserId.ToUpper()} opened a negotiation for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                Body4 = "Go to the negotiation"

            };
            var emailServiceResultBuyer = _emailService.SendEmail(emailBuyer);
            var emailServiceResultSeller = _emailService.SendEmail(emailSeller);

            return await Task.FromResult(result.Succeeded());

        }

        public async Task<OperationResult> SendMessage(NewMessage Command)
        {
            var result = new OperationResult();
            if (!_negotiateRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var negotiate = await _negotiateRepository.Get(Command.NegotiateId);
            var whoIsTheSender = _authenticateHelper.CurrentAccountRole().Id;

            var RedirectUrl =
                $"/dashboard/negotiate/messages/{Command.NegotiateId}";

            var sellerInfo = _userRepository.GetDetail(negotiate.SellerId).Result;
            var buyerInfo = _userApplication.GetDetail(negotiate.BuyerId).Result;

            var listingInfo = _listingRepository.GetListingDetail(negotiate.ListingId);

            var buyyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyerInfo.RoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = sellerInfo.RoleId
            });

            var buyyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    RedirectUrl = RedirectUrl,
                    NotificationBody = $"{sellerInfo.UserId.ToUpper()} has replied to you regarding {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency}",
                    NotificationTitle = ApplicationMessage.NewMessage,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = RedirectUrl,
                    NotificationBody = $"{buyerInfo.UserId.ToUpper()} sends a new message regarding {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency}",
                    NotificationTitle = ApplicationMessage.NewMessage,
                    UserId = negotiate.SellerId
                });

            if (whoIsTheSender == negotiate.BuyerId)
            {
                _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerInfo.RoleId, sellerNotificationId.Result));
            }
            else
            {
                _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyerInfo.RoleId, buyyerNotificationId.Result));
            }
            _recipientRepository.SaveChanges();

            var targetNegotiation = await _negotiateRepository.Get(Command.NegotiateId);
            var filePathString = "";
            if (Command.File != null)
                filePathString = _fileUploader
                    .Uploader(Command.File, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            targetNegotiation.AddMessage(Command.MessageBody, Command.UserId, Command.UserEntity, filePathString);
            _negotiateRepository.SaveChanges();
            return await Task.FromResult(result.Succeeded());
        }
        public async Task<OperationResult> CancelNegotiation(CreateNegotiate Command)
        {
            var result = new OperationResult();
            if (!_negotiateRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var RedirectUrl = $"/dashboard/negotiate/messages/{Command.NegotiateId}";

            var sellerRoleId = _userRepository.GetDetail(Command.SellerId).Result.RoleId;
            var sellerUserId = $"{_userRepository.GetDetail(Command.SellerId).Result.UserId}";
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(Command.BuyerId).Result.RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(Command.BuyerId).Result.UserId}";
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId);


            var buyyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyyerRecipientList.Add(new RecipientViewModel
            {
                UserId = Command.BuyerId,
                IsReed = false,
                RoleId = buyerRoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = Command.SellerId,
                IsReed = false,
                RoleId = sellerRoleId
            });


            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyyerRecipientList,
                    SenderId = Command.BuyerId,
                    RedirectUrl = RedirectUrl,
                    NotificationBody =
                        $"Negotiation CANCELED for {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency} by {(_authenticateHelper.CurrentAccountRole().Id == Command.SellerId ? sellerUserId : buyerUserId)}",
                    NotificationTitle = ApplicationMessage.CanceledNegotiationRequest,
                    UserId = Command.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = Command.SellerId,
                    RedirectUrl = RedirectUrl,
                    NotificationBody = $"Negotiation CANCELED for {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency} by {(_authenticateHelper.CurrentAccountRole().Id == Command.SellerId ? sellerUserId : buyerUserId)}",
                    NotificationTitle = ApplicationMessage.CanceledNegotiationRequest,
                    UserId = Command.SellerId
                });

            _recipientRepository.Create(new Recipient(Command.BuyerId, buyerRoleId, buyerNotificationId.Result));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.SaveChanges();



            var target = _negotiateRepository.Get(Command.NegotiateId).Result;
            target.Canceled();
            if (target.QuatationSent)
            {
                var deal = _dealRepository.Get((long)target.DealId).Result;
                deal.CancelDeal();
                _dealRepository.SaveChanges();
            }

            _negotiateRepository.SaveChanges();


            if (target.QuatationSent)
            {
                var dealAmount = _dealRepository.Get(Convert.ToInt64(target.DealId)).Result.Amount;
                var listDomainModel = _listingRepository.Get(Command.ListingId);
                listDomainModel.Result
                    .Increment($"Deal canceled for {buyerUserId}", dealAmount
                        , Convert.ToInt64(target.DealId), Command.BuyerId);
                _listingRepository.SaveChanges();
            }


            return await Task.FromResult(result.Succeeded());

        }
        public NegotiateViewModel GetNegotiationViewModel(CreateNegotiate Command)
        {
            return _negotiateRepository.GetNegotiationViewModel(Command);
        }
        public NegotiateViewModel GetNegotiationViewModel(long NegotiateId)
        {
            if (_negotiateRepository.Exist(x => x.Id == NegotiateId))
                return _negotiateRepository.GetNegotiationViewModel(NegotiateId);
            return new NegotiateViewModel();
        }
        public List<CreateNegotiate> AllListingItemsBuyyer(long BuyyerId)
        {
            return _negotiateRepository.AllListingItemsBuyyer(BuyyerId);
        }
        public List<CreateNegotiate> AllListingItemsSeller(long SellerId)
        {
            return _negotiateRepository.AllListingItemsSeller(SellerId);
        }
        public List<MessageViewModel> GetMessages(long NegotiateId)
        {
            return _negotiateRepository.GetMessages(NegotiateId);
        }
        public async Task<OperationResult> ActiveNegotiation(long NegotiateId)
        {
            return await _negotiateRepository.ActiveNegotiation(NegotiateId);
        }
    }
}