using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INegotiateRepository _negotiateRepository;
        private readonly IUserNegotiateRepository _userNegotiateRepository;
        private readonly INotificationApplication _notificationApplication;

        public NegotiateApplication(INegotiateRepository negotiateRepository,
            IAutenticateHelper autenticateHelper,
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
            _autenticateHelper = autenticateHelper;
            _listingRepository = listingRepository;
            _recipientRepository = recipientRepository;
            _negotiateRepository = negotiateRepository;
            _userNegotiateRepository = userNegotiateRepository;
            _notificationApplication = notificationApplication;
        }
        public OperationResult Create(CreateNegotiate Command)
        {
            var result = new OperationResult();

            if (!_listingRepository.Exist(x => x.Id == Command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);
            if (_negotiateRepository.Exist(x => x.SellerId == Command.SellerId &
                                              x.BuyerId == Command.BuyerId &
                                              x.ListingId == Command.ListingId &
                                              !x.IsCanceled))
                return result.Failed(ApplicationMessage.DuplicateNegotiation);

            var sellerRoleId = _userRepository.GetDetail(Command.SellerId).RoleId;
            var sellerRoleString = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(Command.BuyerId).RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(Command.BuyerId).UserId}";
            var buyerRoleString = _userApplication.GetUsertypes()
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

            _recipientRepository.Create(new Recipient(Command.BuyerId, buyerRoleId, buyerNotificationId));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerRoleId, sellerNotificationId));
            _recipientRepository.SaveChanges();

            var negotiate = new Negotiate(Command.ListingId, Command.BuyerId, Command.SellerId);
            _negotiateRepository.Create(negotiate);
            _negotiateRepository.SaveChanges();

            _userNegotiateRepository.Create(new UserNegotiate(Command.BuyerId, negotiate.Id, true));
            _userNegotiateRepository.Create(new UserNegotiate(Command.SellerId, negotiate.Id, false));
            _userNegotiateRepository.Save();

            return result.Succeeded();

        }
        public OperationResult SendMessage(NewMessage Command)
        {
            var result = new OperationResult();
            if (!_negotiateRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var negotiate = _negotiateRepository.Get(Command.NegotiateId);
            var whoIsTheSender = _autenticateHelper.CurrentAccountRole().Id;

            var sellerRoleId = _userRepository.GetDetail(negotiate.SellerId).RoleId;
            var sellerRoleString = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyyerRoleId = _userRepository.GetDetail(negotiate.SellerId).RoleId;
            var buyyerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).UserId}";
            var buyyerRoleString = _userApplication.GetUsertypes()
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
                    NotificationBody = $"{sellerRoleString} has replied to you regarding {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.NewMessage,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    NotificationBody = $"{_autenticateHelper.CurrentAccountRole().Email} sends a new message regarding {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.NewMessage,
                    UserId = negotiate.SellerId
                });

            if (whoIsTheSender == negotiate.BuyerId)
            {
                _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId));
            }
            else
            {
                _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyyerRoleId, buyyerNotificationId));
            }
            _recipientRepository.SaveChanges();

            var targetNegotiation = _negotiateRepository.Get(Command.NegotiateId);
            var filePathString = "";
            if (Command.File != null)
                filePathString = _fileUploader
                    .Uploader(Command.File, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            targetNegotiation.AddMessage(Command.MessageBody, Command.UserId, Command.UserEntity, filePathString);
            _negotiateRepository.SaveChanges();
            return result.Succeeded();
        }
        public OperationResult CancelNegotiation(CreateNegotiate Command)
        {
            var result = new OperationResult();
            if (!_negotiateRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var sellerRoleId = _userRepository.GetDetail(Command.SellerId).RoleId;
            var sellerUserId = $"{_userRepository.GetDetail(Command.SellerId).UserId}";
            var sellerRoleString = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(Command.BuyerId).RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(Command.BuyerId).UserId}";
            var buyerRoleString = _userApplication.GetUsertypes()
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
                        $"Negotiation CANCELED for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency} by {(_autenticateHelper.CurrentAccountRole().Id == Command.SellerId ? sellerUserId : buyerUserId)}",
                    NotificationTitle = ApplicationMessage.CanceledNegotiationRequest,
                    UserId = Command.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = Command.SellerId,
                    NotificationBody = $"Negotiation CANCELED for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency} by {(_autenticateHelper.CurrentAccountRole().Id == Command.SellerId ? sellerUserId : buyerUserId)}",
                    NotificationTitle = ApplicationMessage.CanceledNegotiationRequest,
                    UserId = Command.SellerId
                });

            _recipientRepository.Create(new Recipient(Command.BuyerId, buyerRoleId, buyerNotificationId));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerRoleId, sellerNotificationId));
            _recipientRepository.SaveChanges();

            var target = _negotiateRepository.Get(Command.NegotiateId);
            target.Canceled();
            _negotiateRepository.SaveChanges();
            return result.Succeeded();

        }
        public NegotiateViewModel GetNegotiationViewModel(CreateNegotiate Command)
        {
            return _negotiateRepository.GetNegotiationViewModel(Command);
        }
        public NegotiateViewModel GetNegotiationViewModel(long NegotiateId)
        {
            return _negotiateRepository.GetNegotiationViewModel(NegotiateId);
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
        public OperationResult ActiveNegotiation(long NegotiateId)
        {
            return _negotiateRepository.ActiveNegotiation(NegotiateId);
        }
    }
}