using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.User;
using AM.Domain;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class DealRepository : RepositoryBase<long, Deal>, IDealRepository
    {
        private readonly AMContext _amContext;
        public DealRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public Task<List<DealViewModel>> GetAllDeals(long UserId)
        {
            return _amContext.Deals
                .Where(x => x.BuyerId == UserId | x.SellerId == UserId)
                .AsNoTracking()
                .Select(x => new DealViewModel
                {
                    DealId = x.Id,
                    Currency = x.Currency,
                    NegotiateId = x.NegotiateId,
                    Amount = x.Amount,
                    IsCanceled = x.IsDeleted,
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished,
                    IsRejected = x.IsRejected,
                    QuatationSent = x.QuatationSent,
                    PaymentId = x.PaymentInfo.PaymentId,
                    Location = x.Location,
                    TotalCost = x.TotalCost,
                    DeliveryCost = x.DeliveryCost,
                    DeliveryMethod = x.DeliveryMethod,
                    ContractFileString = x.ContractFile,
                    ListingId = x.ListingId,
                    Unit = x.Unit,
                    Listing = new ListingViewModel
                    {
                        Name = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Name,
                        Image = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Image,
                    },
                    Seller = new UserViewModel
                    {
                        Id = x.SellerId,
                        FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).LastName}",
                        Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).Avatar,
                        Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).Email
                    },
                    Buyer = new UserViewModel
                    {
                        Id = x.BuyerId,
                        FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).LastName}",
                        Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).Avatar,
                        Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).Email
                    },
                    TrackingCode = x.TrackingCode,
                    CreationTime = x.CreationTime,
                    DueTime = x.DueTime

                })
                .OrderByDescending(x => x.DealId)
                .ToListAsync();
        }

        public Task<List<DealViewModel>> GetAllFinishedDeals(long UserId)
        {
            return _amContext.Deals
                .Where(x => x.IsFinished)
                .AsNoTracking()
                .Select(x => new DealViewModel
                {
                    DealId = x.Id,
                    Currency = x.Currency,
                    NegotiateId = x.NegotiateId,
                    Amount = x.Amount,
                    IsCanceled = x.IsDeleted,
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished,
                    IsRejected = x.IsRejected,
                    QuatationSent = x.QuatationSent,
                    PaymentId = x.PaymentInfo.PaymentId,
                    PaymentTime = x.PaymentInfo.PaymentTime,
                    Location = x.Location,
                    TotalCost = x.TotalCost,
                    DeliveryCost = x.DeliveryCost,
                    DeliveryMethod = x.DeliveryMethod,
                    ContractFileString = x.ContractFile,
                    ListingId = x.ListingId,
                    Unit = x.Unit,
                    Listing = new ListingViewModel
                    {
                        Name = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Name,
                        Image = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Image,
                    },
                    Seller = new UserViewModel
                    {
                        Id = x.SellerId,
                        FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).LastName}",
                        Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).Avatar,
                        Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.SellerId).Email
                    },
                    Buyer = new UserViewModel
                    {
                        Id = x.BuyerId,
                        FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).LastName}",
                        Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).Avatar,
                        Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == x.BuyerId).Email
                    },
                    TrackingCode = x.TrackingCode,
                    CreationTime = x.CreationTime,
                    DueTime = x.DueTime

                })
                .OrderByDescending(x => x.DealId)
                .ToListAsync();
        }

        public DealViewModel GetDealWithNegotiateId(long NegotiateId)
        {
            var deal = _amContext.Deals
                .Include(x => x.Listing)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefault(x => x.NegotiateId == NegotiateId);

            var negotiate = _amContext.Negotiates
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == NegotiateId);


            var deliveryLocation = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == deal.SellerId).DeliveryLocations
                .FirstOrDefault(x => x.Id == deal.DeliveryLocationId);

            var query = new DealViewModel
            {
                DealId = deal.Id,
                Currency = deal.Currency,
                NegotiateId = deal.NegotiateId,
                Amount = deal.Amount,
                IsCanceled = negotiate.IsCanceled,
                IsActive = deal.IsActive,
                IsFinished = deal.IsFinished,
                IsRejected = deal.IsRejected,
                CreationTime = deal.CreationTime,
                Location = deal.Location,
                ListingId = deal.ListingId,
                TotalCost = deal.TotalCost,
                DeliveryCost = deal.DeliveryCost,
                DeliveryMethod = deal.DeliveryMethod,
                ContractFileString = deal.ContractFile,
                TrackingCode = deal.TrackingCode,
                Buyer = new UserViewModel
                {
                    Id = deal.BuyerId,
                    FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.BuyerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.BuyerId).LastName}",
                    Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.BuyerId).Avatar,
                    Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.BuyerId).Email
                },
                Seller = new UserViewModel
                {
                    Id = deal.SellerId,
                    FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.SellerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.SellerId).LastName}",
                    Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.SellerId).Avatar,
                    Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.SellerId).Email
                },
                Unit = deal.Unit,
                Listing = new ListingViewModel
                {
                    Name = deal.Listing.Name,
                    Image = deal.Listing.Image,
                    Type = deal.Listing.Type,
                    UnitPrice = deal.Listing.UnitPrice,
                    Description = deal.Listing.Description,

                },
                DeliveryLocation = new CreateDeliveryLocation
                {
                    AddressLineOne = deliveryLocation.AddressLineOne,
                    AddressLineTwo = deliveryLocation.AddressLineTwo,
                    City = deliveryLocation.City,
                    Country = deliveryLocation.Country,
                    Latitude = deliveryLocation.Latitude,
                    Longitude = deliveryLocation.Longitude,
                    Name = deliveryLocation.Name,
                    PostalCode = deliveryLocation.PostalCode,
                    LocationId = deliveryLocation.Id

                }
            };

            return query;
        }

        public DealViewModel GetDealWithDealId(long DealId)
        {
            return _amContext.Deals
                    .AsNoTracking()
                    .Where(x => x.Id == DealId).Select(x => new DealViewModel
                    {
                        DealId = x.Id,
                        NegotiateId = x.NegotiateId,
                        ListingId = x.ListingId,
                        PaymentId = x.PaymentInfo.PaymentId,
                        PaymentTime = x.PaymentInfo.PaymentTime,
                        TotalCost = x.PaymentInfo.PaidAmount,
                        TransactionFee = x.PaymentInfo.TransactionFee,
                        TrackingCode = x.TrackingCode,
                        Currency = x.Currency

                    }).First();
        }
        public DealViewModel GetDealWithDealIdforDealIndex(long DealId)
        {
            return _amContext.Deals
                .AsNoTracking()
                .Where(x => x.Id == DealId).Select(x => new DealViewModel
                {
                    DealId = x.Id,
                    NegotiateId = x.NegotiateId,
                    ListingId = x.ListingId,

                }).First();
        }

        public DealViewModel ReturnDealIdWithTrackingRef(string TrackingCode)
        {
            return _amContext.Deals
                .AsNoTracking()
                .Where(x => x.TrackingCode == TrackingCode).Select(x => new DealViewModel
                {
                    DealId = x.Id,
                    NegotiateId = x.NegotiateId,
                    ListingId = x.ListingId
                }).First();
        }
    }
}