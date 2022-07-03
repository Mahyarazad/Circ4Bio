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
using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using DealViewModel = AM.Application.Contracts.Deal.DealViewModel;

namespace AM.Application
{
    public class DealApplication : IDealApplication
    {
        private readonly IMemoryCache _memoryCache;
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
        private readonly ISuppliedItemRepository _suppliedItemRepository;
        private readonly IPurchasedItemRepository _purchasedItemRepository;
        private readonly INotificationApplication _notificationApplication;

        public DealApplication(IDealRepository dealRepository, ISuppliedItemRepository suppliedItemRepository,
            IMemoryCache memoryCache, IPurchasedItemRepository purchasedItemRepository, IUserRepository userRepository
            , IListingApplication listingApplication, IEmailService<EmailModel> emailService
            , IFileUploader fileUploader, IUserApplication userApplication
            , IAuthenticateHelper authenticateHelper, INegotiateRepository negotiateRepository
            , IListingRepository listingRepository, IHttpContextAccessor contextAccessor
            , IRecipientRepository recipientRepository, INotificationApplication notificationApplication)
        {
            _memoryCache = memoryCache;
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
            _suppliedItemRepository = suppliedItemRepository;
            _purchasedItemRepository = purchasedItemRepository;
            _notificationApplication = notificationApplication;
        }

        public Task<OperationResult> CreateQuotation(CreateDeal Command)
        {
            var result = new OperationResult();

            var RedirectUrlSeller = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value.ToString().Replace("Create", "Quotation");
            var RedirectUrlBuyer = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value.ToString().Replace("Create", "ConfirmQuotation");


            var negotiate = _negotiateRepository.Get(Command.NegotiateId).Result;

            var seller = _userRepository.GetDetail(negotiate.SellerId).Result;
            var buyer = _userRepository.GetDetail(negotiate.BuyerId).Result;
            var buyerUserId = $"{_userRepository.GetDetail(negotiate.BuyerId).Result.UserId}";
            var SellerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId);
            var listDomainModel = _listingRepository.Get(Command.ListingId);



            var buyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyer.RoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = seller.RoleId
            });

            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    RedirectUrl = RedirectUrlBuyer,
                    NotificationBody =
                        $"{SellerUserId} send a quotation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = RedirectUrlSeller,
                    NotificationBody =
                        $"You have issued a new quotation for {listingInfo.Result.Name} with {buyerUserId}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.SellerId
                });

            _recipientRepository.Create(new Recipient(negotiate.SellerId, seller.RoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyer.RoleId, buyerNotificationId.Result));
            _recipientRepository.SaveChanges();



            var trackingCode = "Draft Quotation";
            var filePathString = _fileUploader
                .Uploader(Command.ContractFile!, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            var deal = new Deal(Command.DeliveryCost, Command.DeliveryMethod, Command.TotalCost, listingInfo.Result.Unit
                , Command.Currency, Command.Amount, Convert.ToInt32(Command.Location), trackingCode,
                filePathString, Command.DueTime, Command.ListingId, Command.NegotiateId, negotiate.BuyerId
                , negotiate.SellerId);



            deal.QuotationHasSent();
            _dealRepository.Create(deal);
            _dealRepository.SaveChanges();


            negotiate.QuotationHasSent();
            negotiate.AttachDealId(deal.Id);
            listDomainModel.Result.Decrement($"Quotation for {buyerUserId}", Command.Amount, deal.Id,
                negotiate.BuyerId);

            _negotiateRepository.SaveChanges();
            var activateNegotiateResult =
                _negotiateRepository.ActiveNegotiation(Command.NegotiateId).Result;

            if (activateNegotiateResult.IsSucceeded)
            {
                _emailService.SendEmail(new EmailModel
                {
                    EmailTemplate = EmailType.QuotationCreated,
                    Title = ApplicationMessage.QuotationCreated,
                    Body = ApplicationMessage.QuotationCreated,
                    Body1 = $"You have issued a new quotation for {listingInfo.Result.Name} with {buyerUserId}, Total cost is {Command.TotalCost} {Command.Currency}. You can modify your quotion anytime or abide to this deal",
                    Body2 = "Check out The Quotation",
                    Body3 = RedirectUrlSeller,
                    Recipient = seller.Email
                });

                _emailService.SendEmail(new EmailModel
                {
                    EmailTemplate = EmailType.QuotationCreated,
                    Title = ApplicationMessage.QuotationCreated,
                    Body = ApplicationMessage.QuotationCreated,
                    Body1 = $"{SellerUserId} send a quotation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}. You can ask the seller to modify this quotion for you. This is not the final deal.",
                    Body2 = "Check out The Quotation",
                    Body3 = RedirectUrlBuyer,
                    Recipient = buyer.Email
                });

                return Task.FromResult(result.Succeeded());
            }

            return Task.FromResult(result.Failed(ApplicationMessage.SomethingWentWrong));

        }

        public async Task<OperationResult> CreateDeal(CreateDeal Command)
        {
            var result = new OperationResult();


            var negotiate = await _negotiateRepository.Get(Command.NegotiateId);

            var sellerRoleId = _userRepository.GetDetail(negotiate.SellerId).Result.RoleId;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(negotiate.BuyerId).Result.RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(negotiate.BuyerId).Result.UserId}";
            var SellerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId);

            var buyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyerRoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = sellerRoleId
            });

            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    NotificationBody =
                        $"{SellerUserId} send a quotation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    NotificationBody =
                        $"You have issued a new quotation for {listingInfo.Result.Name} with {buyerUserId}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.SellerId
                });

            _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyerRoleId, buyerNotificationId.Result));
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

        public Task<OperationResult> EditDeal(DealViewModel Command)
        {
            var result = new OperationResult();
            if (!_dealRepository.Exist(x => x.Id == Command.DealId))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            var quotation = _dealRepository.Get(Command.DealId).Result;

            var filePathString = _fileUploader
                .Uploader(Command.ContractFile, $"Deal Documents/{Command.NegotiateId}",
                    Guid.NewGuid().ToString());

            var RedirectUrlSeller = _contextAccessor.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer")
                .Value;
            var RedirectUrlBuyer = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Referer").Value.ToString().Replace("Quotation", "ConfirmQuotation");

            ////Notification Push
            var negotiate = _negotiateRepository.Get(Command.NegotiateId).Result;

            var sellerRoleId = _userRepository.GetDetail(negotiate.SellerId).Result.RoleId;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(negotiate.BuyerId).Result.RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(negotiate.BuyerId).Result.UserId}";
            var SellerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId);

            var buyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyerRoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = sellerRoleId
            });

            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    RedirectUrl = RedirectUrlBuyer,
                    NotificationBody =
                        $"{SellerUserId} update the quotation for {listingInfo.Result.Name} to you. Total cost is {Command.TotalCost} {Command.Currency}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = RedirectUrlSeller,
                    NotificationBody =
                        $"You have updated the quotation for {listingInfo.Result.Name} with {buyerUserId}",
                    NotificationTitle = ApplicationMessage.DealsCreated,
                    UserId = negotiate.SellerId
                });

            _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyerRoleId, buyerNotificationId.Result));
            _recipientRepository.SaveChanges();





            quotation.Edit(Command.DeliveryCost, Command.DeliveryMethod, Command.TotalCost, Command.Unit
                , Command.Currency, Command.Amount, Convert.ToInt32(Command.DeliveryLocation.LocationId),
                filePathString);
            _dealRepository.SaveChanges();

            return Task.FromResult(result.Succeeded());
        }

        public Task<OperationResult> RejectDeal(long Id)
        {
            var result = new OperationResult();
            return Task.FromResult(result);
        }

        public Task<OperationResult> FinishDeal(DealViewModel Command)
        {
            var result = new OperationResult();



            var deal = _dealRepository.Get(Command.DealId).Result;
            deal.PaymentFinished(new PaymentInfo(Command.PaymentId, Command.PaymentTime, Command.PayerEmail,
                Command.PayerFirstName, Command.PayerLastName, Command.PaidAmount, Command.TransactionFee));
            _dealRepository.SaveChanges();

            var returnUrl = $"/Dashboard/PaymentInfo/{deal.Id}";

            var negotiate = _negotiateRepository.Get(Command.NegotiateId).Result;
            var sellerRoleId = _userRepository.GetDetail(negotiate.SellerId).Result.RoleId;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == sellerRoleId).TypeName;

            var buyerRoleId = _userRepository.GetDetail(negotiate.BuyerId).Result.RoleId;
            var buyerUserId = $"{_userRepository.GetDetail(negotiate.BuyerId).Result.UserId}";
            var SellerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyerRoleId).TypeName;

            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId).Result;

            var buyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyerRoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = sellerRoleId
            });


            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    RedirectUrl = returnUrl,
                    NotificationBody =
                        $"Payment has been done for {deal.TrackingCode}, Payment Id: {Command.PaymentId}, Total Paid Amount: {deal.TotalCost}",
                    NotificationTitle = ApplicationMessage.PaymentDone,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = returnUrl,
                    NotificationBody =
                        $"Payment has been done for {deal.TrackingCode}, Payment Id: {Command.PaymentId}, Total Paid Amount: {deal.TotalCost}",
                    NotificationTitle = ApplicationMessage.PaymentDone,
                    UserId = negotiate.SellerId
                });

            negotiate.Finished();
            _negotiateRepository.SaveChanges();
            _recipientRepository.Create(new Recipient(negotiate.SellerId, sellerRoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyerRoleId, buyerNotificationId.Result));

            _recipientRepository.SaveChanges();
            SuppliedItem(new SuppliedItemModel
            {
                UserId = negotiate.SellerId,
                Amount = negotiate.Deal.Amount,
                ListingId = negotiate.ListingId,
                Currency = negotiate.Deal.Currency,
                TotalPaid = Command.PaidAmount,
                UnitPrice = listingInfo.UnitPrice,

            });

            PurchasedItem(new PurchasedItemModel
            {
                UserId = negotiate.BuyerId,
                Amount = negotiate.Deal.Amount,
                ListingId = negotiate.ListingId,
                Currency = negotiate.Deal.Currency,
                TotalPaid = Command.PaidAmount,
                UnitPrice = listingInfo.UnitPrice,
            });
            return Task.FromResult(result);
        }

        public Task<OperationResult> AtivateDeal(DealViewModel Command)
        {
            var result = new OperationResult();

            var RedirectUrl = $"/Dashboard/Deals/ConfirmQuotation/{Command.NegotiateId}";

            var negotiate = _negotiateRepository.Get(Command.NegotiateId).Result;
            var seller = _userRepository.GetDetail(negotiate.SellerId).Result;
            var sellerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == seller.RoleId).TypeName;

            var buyer = _userRepository.GetDetail(negotiate.BuyerId).Result;
            var buyerUserId = $"{_userRepository.GetDetail(negotiate.BuyerId).Result.UserId}";
            var SellerUserId = $"{_userRepository.GetDetail(negotiate.SellerId).Result.UserId}";
            var buyerRoleString = _userApplication.GetUsertypes().Result
                .FirstOrDefault(x => x.TypeId == buyer.RoleId).TypeName;


            var listingInfo = _listingRepository.GetListingDetail(Command.ListingId).Result;

            var buyerRecipientList = new List<RecipientViewModel>();
            var sellerRecipientList = new List<RecipientViewModel>();

            buyerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.BuyerId,
                IsReed = false,
                RoleId = buyer.RoleId
            });

            sellerRecipientList.Add(new RecipientViewModel
            {
                UserId = negotiate.SellerId,
                IsReed = false,
                RoleId = seller.RoleId
            });

            var trackingCode = CodeGenerator.Generate($"#{listingInfo.Type.Substring(0, 3).ToUpper()}");
            var deal = _dealRepository.Get(Command.DealId);
            deal.Result.ActivateDeal(trackingCode);
            _dealRepository.SaveChanges();

            var buyerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = buyerRecipientList,
                    SenderId = negotiate.BuyerId,
                    RedirectUrl = RedirectUrl,
                    NotificationBody =
                        $"You have confirmed the quotation for {listingInfo.Name}, you can track your deal with the tracking code: {trackingCode}",
                    NotificationTitle = ApplicationMessage.ActiveDeal,
                    UserId = negotiate.BuyerId
                });

            var sellerNotificationId = _notificationApplication
                .PushNotification(new NotificationViewModel
                {
                    RecipientList = sellerRecipientList,
                    SenderId = negotiate.SellerId,
                    RedirectUrl = RedirectUrl,
                    NotificationBody =
                        $"{buyerUserId} have confirmed the quotation for {listingInfo.Name}, you can track your deal with the tracking code: {trackingCode}",
                    NotificationTitle = ApplicationMessage.ActiveDeal,
                    UserId = negotiate.SellerId
                });

            negotiate.QuotationConfirmed();
            _negotiateRepository.SaveChanges();
            _recipientRepository.Create(new Recipient(negotiate.SellerId, seller.RoleId, sellerNotificationId.Result));
            _recipientRepository.Create(new Recipient(negotiate.BuyerId, buyer.RoleId, buyerNotificationId.Result));
            _recipientRepository.SaveChanges();

            _emailService.SendEmail(new EmailModel
            {
                EmailTemplate = EmailType.ActiveDeal,
                Title = ApplicationMessage.ActiveDeal,
                Body = ApplicationMessage.ActiveDeal,
                Body1 = $"{buyerUserId} have confirmed the quotation for {listingInfo.Name}, you can track your deal with the tracking code: {trackingCode}",
                Body2 = ApplicationMessage.ActiveDealDetail,
                Body3 = RedirectUrl,
                Body4 = "Open Your Deal Status",
                Recipient = seller.Email
            });

            _emailService.SendEmail(new EmailModel
            {
                EmailTemplate = EmailType.ActiveDeal,
                Title = ApplicationMessage.ActiveDeal,
                Body = ApplicationMessage.ActiveDeal,
                Body1 = $"You have confirmed the quotation for {listingInfo.Name}, you can track your deal with the tracking code: {trackingCode}",
                Body2 = ApplicationMessage.ActiveDealDetail,
                Body3 = RedirectUrl,
                Body4 = "Open Your Deal Status",
                Recipient = buyer.Email
            });

            return Task.FromResult(result.Succeeded());
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

        public Task<List<DealViewModel>> GetAllDeals()
        {
            return _dealRepository.GetAllDeals();
        }

        public Task<List<DealViewModel>> GetAllFinishedDeals(long UserId)
        {
            return _dealRepository.GetAllFinishedDeals(UserId);
        }

        public DealViewModel GetDealWithNegotiateId(long NegotiateId)
        {
            if (_dealRepository.Exist(x => x.NegotiateId == NegotiateId))
            {
                return _dealRepository.GetDealWithNegotiateId(NegotiateId);
            }
            else
            {
                return new DealViewModel();
            }

        }

        public DealViewModel GetDealWithDealId(long DealId)
        {
            if (_dealRepository.Exist(x => x.Id == DealId))
            {
                return _dealRepository.GetDealWithDealId(DealId);
            }
            else
            {
                return new DealViewModel();
            }

        }

        public DealViewModel GetDealWithDealIdforDealIndex(long DealId)
        {
            if (_dealRepository.Exist(x => x.Id == DealId))
            {
                return _dealRepository.GetDealWithDealIdforDealIndex(DealId);
            }
            else
            {
                return new DealViewModel();
            }
        }

        public DealViewModel ReturnDealIdWithTrackingRef(string TrackingCode)
        {
            if (_dealRepository.Exist(x => x.TrackingCode == TrackingCode))
            {
                return _dealRepository.ReturnDealIdWithTrackingRef(TrackingCode);
            }
            else
            {
                return new DealViewModel();
            }
        }

        public Task<OperationResult> SuppliedItem(SuppliedItemModel Command)
        {
            var result = new OperationResult();
            if (_listingRepository.Exist(x => x.Id == Command.ListingId))
            {
                _suppliedItemRepository
                    .Create(new SuppliedItem(Command.ListingId, Command.UserId, Command.Amount
                        , Command.UnitPrice, Command.TotalPaid, Command.Currency));
                _suppliedItemRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
            else
            {
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            }

        }

        public Task<OperationResult> PurchasedItem(PurchasedItemModel Command)
        {
            var result = new OperationResult();
            if (_listingRepository.Exist(x => x.Id == Command.ListingId))
            {
                _purchasedItemRepository
                    .Create(new PurchasedItem(Command.ListingId, Command.UserId, Command.Amount
                        , Command.UnitPrice, Command.TotalPaid, Command.Currency));
                _purchasedItemRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
            else
            {
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            }
        }

        public Task<List<PurchasedItemModel>> GetPurchasedList(long UserId)
        {
            var CacheKey = "PurchasedList";
            if (!_memoryCache.TryGetValue(CacheKey, out List<PurchasedItemModel> Items))
            {
                Items = _purchasedItemRepository.GetPurchasedList(UserId);
                var CacheExpirayOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                    Priority = CacheItemPriority.Normal
                };
                _memoryCache.Set(CacheKey, Items, CacheExpirayOption);
                return Task.FromResult(Items);
            }
            else
            {
                return Task.FromResult(_memoryCache.Get<List<PurchasedItemModel>>(CacheKey));
            }
        }

        public Task<List<SuppliedItemModel>> GetSuppliedList(long UserId)
        {
            var CacheKey = "SuppliedList";
            if (!_memoryCache.TryGetValue(CacheKey, out List<SuppliedItemModel> Items))
            {
                Items = _suppliedItemRepository.GetSuppliedList(UserId);
                var CacheExpirayOption = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(50),
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                    Priority = CacheItemPriority.Normal
                };
                _memoryCache.Set(CacheKey, Items, CacheExpirayOption);
                return Task.FromResult(Items);
            }
            else
            {
                return Task.FromResult(_memoryCache.Get<List<SuppliedItemModel>>(CacheKey));
            }
        }
    }
}