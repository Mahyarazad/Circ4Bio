﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Domain.ListingAggregate;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using AM.Domain.NotificationAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;

namespace AM.Application
{
    public class ListingApplication : IListingApplication
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IFileUploader _fileUploader;
        private readonly IUserRepository _userRepository;
        private readonly IUserApplication _userApplication;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IListingRepository _listingRepository;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INotificationApplication _notificationApplication;

        public ListingApplication(IListingRepository listingRepository,
            INotificationApplication notificationApplication,
            IHttpContextAccessor contextAccessor,
            IMemoryCache memoryCache,
            IRecipientRepository recipientRepository,
            IUserRepository userRepository,
            IAuthenticateHelper authenticateHelper,
            IUserApplication userApplication,
            IFileUploader fileUploader)
        {
            _memoryCache = memoryCache;
            _fileUploader = fileUploader;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _userApplication = userApplication;
            _listingRepository = listingRepository;
            _authenticateHelper = authenticateHelper;
            _recipientRepository = recipientRepository;
            _notificationApplication = notificationApplication;
        }
        public Task<OperationResult> Create(CreateListing command)
        {
            var result = new OperationResult();
            var issuerId = _authenticateHelper.CurrentAccountRole().Id;
            var roleId = Convert.ToInt32(_authenticateHelper.CurrentAccountRole().RoleId);
            var typeofListing = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == roleId).TypeName;

            var RedirectUrl = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value
                .ToString()
                .Replace("Listing", "Availablelisting")
                .Replace("Create", "Detail");

            command.ImageString = _fileUploader
                    .Uploader(command.Image, $"Listing_Images/{typeofListing}", Guid.NewGuid().ToString());
            if (command.Image == null)
                command.ImageString = "listing-default.png";

            string notificationTitle = null;
            switch (roleId)
            {
                case 1:
                    notificationTitle = ApplicationMessage.ListingAdmin;
                    break;
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

            var listing = new Listing(command.Name, typeofListing, command.Description, command.ImageString,
                command.DeliveryMethod, command.Unit, command.UnitPrice, command.Amount, command.Status,
                issuerId, command.IsService, command.Currency, command.Location);

            _listingRepository.Create(listing);
            _listingRepository.SaveChanges();

            var recipientList = _userApplication.GetUserListForListing(issuerId).Result;

            if (command.IsService)
            {
                command.Amount = 0;
            }

            var notificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = _userApplication.GetUserListForListing(issuerId).Result,
                    SenderId = issuerId,
                    RedirectUrl = RedirectUrl + $"/{listing.Id}",
                    NotificationBody = $"{command.Name} is available now in Listing, Available amount is " +
                                       $"{command.Amount.ToString()} {command.Unit.ToString()} at {command.UnitPrice.ToString()}",
                    NotificationTitle = notificationTitle,
                    UserId = issuerId
                });

            foreach (var receiver in recipientList)
            {
                _recipientRepository.Create(new Recipient(receiver.UserId, receiver.RoleId, notificationId.Result));
            }

            _recipientRepository.SaveChanges();
            var systemNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    SenderId = 1,
                    RedirectUrl = RedirectUrl + $"/{listing.Id}",
                    NotificationBody = ApplicationMessage.ListingNewItemListed,
                    NotificationTitle = ApplicationMessage.SystemMessage,
                    UserId = issuerId
                });

            _recipientRepository.Create(new Recipient(issuerId, roleId, systemNotificationId.Result));
            _recipientRepository.SaveChanges();


            return Task.FromResult(result.Succeeded());
        }
        public Task<OperationResult> Edit(EditListing command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.Id))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));

            var roleId = Convert.ToInt64(_authenticateHelper.CurrentAccountRole().RoleId);
            var typeofListing = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == roleId).TypeName;

            var imageFileName = _fileUploader
                .Uploader(command.Image, $"Listing_Images/{typeofListing}", Guid.NewGuid().ToString());

            var target = _listingRepository.Get(command.Id);
            target.Result.Edit(command.Name, command.Description, imageFileName,
                command.DeliveryMethod, command.Unit, command.UnitPrice, command.Currency, command.Location);
            _listingRepository.SaveChanges();

            return Task.FromResult(result.Succeeded());
        }
        public Task<List<ListingViewModel>> GetUserListing(long id)
        {
            return _listingRepository.GetUserListing(id);
        }
        public Task<List<ListingViewModel>> GetAllListing()
        {
            var CacheKey = "MarketListing";
            if (!_memoryCache.TryGetValue(CacheKey, out List<ListingViewModel> marketListing))
            {
                marketListing = _listingRepository.GetAllListing().Result;

                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20)
                };

                _memoryCache.Set(CacheKey, marketListing, cacheExpiryOptions);
                return Task.FromResult(marketListing);
            }
            return Task.FromResult(_memoryCache.Get<List<ListingViewModel>>("MarketListing"));
        }

        public Task<List<ListingViewModel>> GetAllListingForAdmin()
        {
            return _listingRepository.GetAllListingForAdmin();
        }

        public Task<long> GetOwnerUserID(long id)
        {
            return _listingRepository.GetOwnerUserID(id);
        }

        public Task<List<ListingViewModel>> GetDeletedUserListing(long id)
        {
            return _listingRepository.GetUserListing(id);
        }

        public Task<ListingViewModel> GetDetailListing(long id)
        {
            if (_listingRepository.Exist(x => x.Id == id))
                return _listingRepository.GetDetailListing(id);
            return Task.FromResult(new ListingViewModel());
        }

        public Task<EditListing> GetEditListing(long listingId)
        {
            if (_listingRepository.Exist(x => x.Id == listingId))
                return _listingRepository.GetListingDetail(listingId);
            return Task.FromResult(new EditListing());
        }

        public Task<OperationResult> Delete(long id)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == id))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            var target = _listingRepository.Get(id);

            if (target.Result != null)
            {
                if (target.Result.Image != "listing-default.png")
                {
                    var deleteFileResult =
                        _fileUploader.DeleteFile(target.Result.Type!, target.Result.Image!);
                    if (deleteFileResult.IsSucceeded)
                    {
                        target.Result.MarkDeleted();
                        _listingRepository.SaveChanges();
                        return Task.FromResult(result.Succeeded());
                    }

                    return Task.FromResult(deleteFileResult);
                }

                // Skip the default listing Image
                target.Result.MarkDeleted();
                _listingRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());

            }

            return Task.FromResult(result.Failed(ApplicationMessage.SomethingWentWrong));
        }

        public Task<OperationResult> IncrementAmount(InputAmount command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.ListingId))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));

            var target = _listingRepository.Get(command.ListingId);
            if (command.DealId != 0)
            {
                target.Result.Increment(command.Description, command.Count, command.DealId, command.UserId);
            }
            else
            {
                target.Result.Increment(command.Description, command.Count, 0, command.UserId);
            }
            _listingRepository.SaveChanges();

            return Task.FromResult(result.Succeeded());

        }
        public Task<OperationResult> DecrementAmount(InputAmount command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == command.ListingId))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));

            var target = _listingRepository.Get(command.ListingId);
            if (command.DealId != 0)
            {
                if (target.Result.Amount - command.Count < 0)
                    return Task.FromResult(result.Failed(ApplicationMessage.CannotDeduct));
                target.Result.Decrement(command.Description, command.Count, command.DealId, command.UserId);
            }
            else
            {
                if (target.Result.Amount - command.Count < 0)
                    return Task.FromResult(result.Failed(ApplicationMessage.CannotDeduct));
                target.Result.Decrement(command.Description, command.Count, 0, command.UserId);
            }
            _listingRepository.SaveChanges();

            return Task.FromResult(result.Succeeded());
        }
        public async Task<List<ListingOperationLog>> GetListingOperationLog(long id)
        {
            var result = _listingRepository.Get(id).Result.ListingOperations
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
                }).OrderByDescending(x => x.Id);
            return result.ToList();

        }
        public Task<OperationResult> MarkPrivate(long id)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == id))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            var target = _listingRepository.Get(id);
            target.Result.MarkPrivate();
            _listingRepository.SaveChanges();
            return Task.FromResult(result.Succeeded());
        }
        public Task<OperationResult> MarkPublic(long id)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == id))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            var target = _listingRepository.Get(id);
            target.Result.MarkPublic();
            _listingRepository.SaveChanges();
            return Task.FromResult(result.Succeeded());
        }
        public Task<List<ActiveListing>> GetActiveListing(long userId)
        {
            return _listingRepository.GetActiveListing(userId);
        }

        public long LastCreatedListingId()
        {
            return _listingRepository.GetTheLastListingId();
        }
    }
}