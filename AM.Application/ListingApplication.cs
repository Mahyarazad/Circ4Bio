using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Domain.ListingAggregate;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Domain.NotificationAggregate;

namespace AM.Application
{
    public class ListingApplication : IListingApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IUserApplication _userApplication;
        private readonly IListingRepository _listingRepository;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INotificationApplication _notificationApplication;

        public ListingApplication(IListingRepository listingRepository,
            INotificationApplication notificationApplication,
            IRecipientRepository recipientRepository,
            IAutenticateHelper autenticateHelper,
            IUserApplication userApplication,
            IFileUploader fileUploader)
        {
            _fileUploader = fileUploader;
            _userApplication = userApplication;
            _listingRepository = listingRepository;
            _autenticateHelper = autenticateHelper;
            _recipientRepository = recipientRepository;
            _notificationApplication = notificationApplication;
        }

        public OperationResult Create(CreateListing command)
        {
            var result = new OperationResult();
            var issuerId = _autenticateHelper.CurrentAccountRole().Id;
            var roleId = Convert.ToInt32(_autenticateHelper.CurrentAccountRole().RoleId);
            var typeofListing = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == roleId).TypeName;

            command.ImageString = _fileUploader
                    .Uploader(command.Image, $"Listing_Images/{typeofListing}", Guid.NewGuid().ToString());
            if (command.Image == null)
                command.ImageString = "listing-default.png";

            string notificationTitle = null;
            switch (roleId)
            {
                case 2:
                    notificationTitle = ApplicationMessage.ListingTechnologyProvider;
                    break;
                case 3:
                    notificationTitle = ApplicationMessage.ListingPlantOwner;
                    break;
                case 4:
                    notificationTitle = ApplicationMessage.ListingSupplierRawMaterial;
                    break;
                default:
                    break;
            }

            var recipientList = _userApplication.GetUserListForListing(issuerId);

            if (command.IsService)
            {
                command.Amount = 0;
            }

            var notificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = _userApplication.GetUserListForListing(issuerId),
                    SenderId = issuerId,
                    NotificationBody = $"{command.Name} is available now in Listing, Available amount is " +
                                       $"{command.Amount.ToString()} {command.Unit.ToString()} at {command.UnitPrice.ToString()}",
                    NotificationTitle = notificationTitle,
                    UserId = issuerId
                });

            foreach (var receiver in recipientList)
            {
                _recipientRepository.Create(new Recipient(receiver.UserId, receiver.RoleId, notificationId));
            }
            _recipientRepository.SaveChanges();

            var systemNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    SenderId = 1,
                    NotificationBody = ApplicationMessage.ListingNewItemListed,
                    NotificationTitle = ApplicationMessage.SystemMessage,
                    UserId = issuerId
                });

            _recipientRepository.Create(new Recipient(issuerId, roleId, systemNotificationId));
            _recipientRepository.SaveChanges();



            var listing = new Listing(command.Name, typeofListing, command.Description, command.ImageString,
                command.DeliveryMethod, command.Unit, command.UnitPrice, command.Amount, command.Status,
                issuerId, command.IsService);

            _listingRepository.Create(listing);
            _listingRepository.SaveChanges();

            return result.Succeeded();
        }

        public OperationResult Edit(EditListing command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.Id))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var roleId = Convert.ToInt64(_autenticateHelper.CurrentAccountRole().RoleId);
            var typeofListing = _userApplication.GetUsertypes()
                .FirstOrDefault(x => x.TypeId == roleId).TypeName;

            var imageFileName = _fileUploader.Uploader(command.Image, $"Listing_Images/${typeofListing}", command.Name);

            var target = _listingRepository.Get(command.Id);
            target.Edit(command.Name, command.Description, imageFileName,
                command.DeliveryMethod, command.Unit, command.UnitPrice);
            _listingRepository.SaveChanges();

            return result.Succeeded();
        }

        public List<ListingViewModel> GetUserListing(long id)
        {
            return _listingRepository.GetUserListing(id);
        }
        public List<ListingViewModel> GetAllListing()
        {
            return _listingRepository.GetAllListing();
        }

        public List<ListingViewModel> GetDeletedUserListing(long id)
        {
            return _listingRepository.GetUserListing(id);
        }

        public EditListing GetEditListing(long listingId)
        {
            return _listingRepository.GetListingDetail(listingId);
        }

        public OperationResult Delete(long id)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _listingRepository.Get(id);
            target.MarkDeleted();
            _listingRepository.SaveChanges();
            return result.Succeeded();

        }

        public OperationResult IncrementAmount(InputAmount command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var target = _listingRepository.Get(command.ListingId);
            if (command.DealId != 0)
            {
                target.Increment(command.Description, command.Count, command.DealId, command.UserId);
            }
            else
            {
                target.Increment(command.Description, command.Count, 0, command.UserId);
            }
            _listingRepository.SaveChanges();

            return result.Succeeded();

        }

        public OperationResult DeccrementAmount(InputAmount command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var target = _listingRepository.Get(command.ListingId);
            if (command.DealId != 0)
            {
                if (target.Amount - command.Count < 0)
                    return result.Failed(ApplicationMessage.CannotDeduct);
                target.Decrement(command.Description, command.Count, command.DealId, command.UserId);
            }
            else
            {
                if (target.Amount - command.Count < 0)
                    return result.Failed(ApplicationMessage.CannotDeduct);
                target.Decrement(command.Description, command.Count, 0, command.UserId);
            }
            _listingRepository.SaveChanges();

            return result.Succeeded();
        }

        public List<ListingOperationLog> GetListingOperationLog(long id)
        {
            return _listingRepository.Get(id).ListingOperations
                .Select(x => new ListingOperationLog
                {
                    UserId = x.UserId,
                    Count = x.Count,
                    CurrentAmount = x.CurrentAmount,
                    DealId = x.DealId,
                    Description = x.Description,
                    ListingId = x.ListingId,
                    OperationTime = x.OperationTime,
                    OperationType = x.OperationType,
                    Id = x.Id
                }).OrderByDescending(x => x.Id)
                .ToList();
        }

        public OperationResult MarkPrivate(long id)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _listingRepository.Get(id);
            target.MarkPrivate();
            _listingRepository.SaveChanges();
            return result.Succeeded();
        }

        public OperationResult MarkPublic(long id)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _listingRepository.Get(id);
            target.MarkPublic();
            _listingRepository.SaveChanges();
            return result.Succeeded();
        }


    }
}