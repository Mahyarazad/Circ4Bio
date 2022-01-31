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
        private readonly IUserRepository _userRepository;
        private readonly IUserApplication _userApplication;
        private readonly IListingRepository _listingRepository;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INegotiateRepository _negotiateRepository;
        private readonly INotificationApplication _notificationApplication;

        public NegotiateApplication(INegotiateRepository negotiateRepository,
            IAutenticateHelper autenticateHelper,
            IRecipientRepository recipientRepository,
            INotificationApplication notificationApplication,
            IUserApplication userApplication,
            IListingRepository listingRepository,
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
            _listingRepository = listingRepository;
            _recipientRepository = recipientRepository;
            _negotiateRepository = negotiateRepository;
            _notificationApplication = notificationApplication;
        }

        public OperationResult Create(CreateNegotiate Command)
        {
            var result = new OperationResult();

            if (!_listingRepository.Exist(x => x.Id == Command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var sellerRoleId = _userRepository.GetDetail(Command.SellerId).RoleId;
            var sellerRoleString = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyyerRoleId = _userRepository.GetDetail(Command.SellerId).RoleId;
            var buyyerUserId = $"{_userRepository.GetDetail(Command.SellerId).UserId}";
            var buyyerRoleString = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == buyyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId);

            var buyyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyyerRecipientList.Add(new RecipientViewModel
            {
                UserId = Command.BuyyerId,
                IsReed = false,
                RoleId = buyyerRoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = Command.SellerId,
                IsReed = false,
                RoleId = sellerRoleId
            });

            var buyyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyyerRecipientList,
                    SenderId = Command.BuyyerId,
                    NotificationBody = $"Negotiation opened for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.SubmitNegotiationRequest,
                    UserId = Command.BuyyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = Command.SellerId,
                    NotificationBody = $"{buyyerUserId} opened a negotiation for {listingInfo.Name} at {listingInfo.UnitPrice} {listingInfo.Currency}",
                    NotificationTitle = ApplicationMessage.ReceivedNegotiation,
                    UserId = Command.SellerId
                });

            _recipientRepository.Create(new Recipient(Command.BuyyerId, buyyerRoleId, buyyerNotificationId));
            _recipientRepository.Create(new Recipient(Command.SellerId, sellerRoleId, sellerNotificationId));
            _recipientRepository.SaveChanges();


            _negotiateRepository.Create(new Negotiate(Command.ListingId, Command.BuyyerId, Command.SellerId));
            _negotiateRepository.SaveChanges();
            return result.Succeeded();

        }

        public OperationResult SendMessage(NewMessage Command)
        {
            var result = new OperationResult();
            if (!_negotiateRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var targetNegotiation = _negotiateRepository.Get(Command.NegotiateId);
            targetNegotiation.AddMessage(Command.MessageBody, Command.UserId, Command.UserEntity);
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
    }
}