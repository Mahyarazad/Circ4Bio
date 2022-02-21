using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Domain.ListingAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.NotificationAggregate;
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;

namespace AM.Application
{
    public class NegotiateApplication : INegotiateApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IUserRepository _userRepository;
        private readonly IUserApplication _userApplication;
        private readonly IListingRepository _listingRepository;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INegotiateRepository _negotiateRepository;
        private readonly IUserNegotiateRepository _userNegotiateRepository;
        private readonly INotificationApplication _notificationApplication;

        public NegotiateApplication(INegotiateRepository negotiateRepository,
            IAuthenticateHelper authenticateHelper,
            IRecipientRepository recipientRepository,
            INotificationApplication notificationApplication,
            IUserApplication userApplication,
            IListingRepository listingRepository,
            IUserNegotiateRepository userNegotiateRepository,
            IFileUploader fileUploader,
            IUserRepository userRepository)
        {
            _fileUploader = fileUploader;
            _userRepository = userRepository;
            _userApplication = userApplication;
            _authenticateHelper = authenticateHelper;
            _listingRepository = listingRepository;
            _recipientRepository = recipientRepository;
            _negotiateRepository = negotiateRepository;
            _userNegotiateRepository = userNegotiateRepository;
            _notificationApplication = notificationApplication;
        }
        public async Task<OperationResult> Create(CreateNegotiate Command)
        {
            var result = new OperationResult();

            if (!_listingRepository.Exist(x => x.Id == Command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);
            if (_negotiateRepository.Exist(x => x.SellerId == Command.SellerId &
                                              x.BuyerId == Command.BuyerId &
                                              x.ListingId == Command.ListingId &
                                              !x.IsCanceled))
                return result.Failed(ApplicationMessage.DuplicateNegotiation);

            var sellerRoleId = _userRepository.GetDetail(Command.SellerId).Result.RoleId;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(Command.BuyerId).Result.RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(Command.BuyerId).Result.UserId}";
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyerRoleId).TypeName;

            var listingInfo = await _listingRepository.GetListingDetail(Command.ListingId);

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
                    NotificationBody = $"Negotiation opened for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.SubmitNegotiationRequest,
                    UserId = Command.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = Command.SellerId,
                    NotificationBody = $"{buyerUserId} opened a negotiation for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.ReceivedNegotiation,
                    UserId = Command.SellerId
                });

            _recipientRepository.Create(new Recipient(Command.BuyerId, buyerRoleId, buyerNotificationId.Result));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.SaveChanges();

            var negotiate = new Negotiate(Command.ListingId, Command.BuyerId, Command.SellerId);
            _negotiateRepository.Create(negotiate);
            _negotiateRepository.SaveChanges();

            _userNegotiateRepository.Create(new UserNegotiate(Command.BuyerId, negotiate.Id, true));
            _userNegotiateRepository.Create(new UserNegotiate(Command.SellerId, negotiate.Id, false));
            _userNegotiateRepository.Save();

            return result.Succeeded();

        }
        public async Task<OperationResult> SendMessage(NewMessage Command)
        {
            var result = new OperationResult();
            if (!_negotiateRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var negotiate = await _negotiateRepository.Get(Command.NegotiateId);
            var whoIsTheSender = _authenticateHelper.CurrentAccountRole().Id;

            var sellerRoleId = _userRepository.GetDetail(negotiate.SellerId).Result.RoleId;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyyerRoleId = _userRepository.GetDetail(negotiate.SellerId).Result.RoleId;
            var buyyerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";
            var buyyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(negotiate.ListingId);

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
                    NotificationBody = $"{sellerRoleString} has replied to you regarding {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency}",
                    NotificationTitle = ApplicationMessage.NewMessage,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    NotificationBody = $"{_authenticateHelper.CurrentAccountRole().Email} sends a new message regarding {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency}",
                    NotificationTitle = ApplicationMessage.NewMessage,
                    UserId = negotiate.SellerId
                });

            if (whoIsTheSender == negotiate.BuyerId)
            {
                _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            }
            else
            {
                _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyyerRoleId, buyyerNotificationId.Result));
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
                    NotificationBody = $"Negotiation CANCELED for {listingInfo.Result.Name} at {listingInfo.Result.UnitPrice} {listingInfo.Result.Currency} by {(_authenticateHelper.CurrentAccountRole().Id == Command.SellerId ? sellerUserId : buyerUserId)}",
                    NotificationTitle = ApplicationMessage.CanceledNegotiationRequest,
                    UserId = Command.SellerId
                });

            _recipientRepository.Create(new Recipient(Command.BuyerId, buyerRoleId, buyerNotificationId.Result));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.SaveChanges();

            var target = _negotiateRepository.Get(Command.NegotiateId);
            target.Result.Canceled();
            _negotiateRepository.SaveChanges();
            return result.Succeeded();

        }
        public async Task<NegotiateViewModel> GetNegotiationViewModel(CreateNegotiate Command)
        {
            return await _negotiateRepository.GetNegotiationViewModel(Command);
        }
        public async Task<NegotiateViewModel> GetNegotiationViewModel(long NegotiateId)
        {
            return await _negotiateRepository.GetNegotiationViewModel(NegotiateId);
        }
        public async Task<List<CreateNegotiate>> AllListingItemsBuyyer(long BuyyerId)
        {
            return await _negotiateRepository.AllListingItemsBuyyer(BuyyerId);
        }
        public async Task<List<CreateNegotiate>> AllListingItemsSeller(long SellerId)
        {
            return await _negotiateRepository.AllListingItemsSeller(SellerId);
        }
        public async Task<List<MessageViewModel>> GetMessages(long NegotiateId)
        {
            return await _negotiateRepository.GetMessages(NegotiateId);
        }
        public async Task<OperationResult> ActiveNegotiation(long NegotiateId)
        {
            return await _negotiateRepository.ActiveNegotiation(NegotiateId);
        }
    }
}