using System.Collections.Generic;
using System.Linq;
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

        public List<DealViewModel> GetAllDeals(long UserId)
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
                    IsDeleted = x.IsDeleted,
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished,
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
                .ToList();
        }

        public DealViewModel GetDealWithDealId(long DealId)
        {
            return _amContext.Deals
                .Where(x => x.Id == DealId)
                .AsNoTracking()
                .Select(x => new DealViewModel
                {
                    DealId = x.Id,
                    Currency = x.Currency,
                    NegotiateId = x.NegotiateId,
                    Amount = x.Amount,
                    IsDeleted = x.IsDeleted,
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished,
                    Location = x.Location,
                    TotalCost = x.TotalCost,
                    DeliveryCost = x.DeliveryCost,
                    DeliveryMethod = x.DeliveryMethod,
                    ContractFileString = x.ContractFile,
                    TrackingCode = x.TrackingCode,
                    Unit = x.Unit,
                    Listing = new ListingViewModel
                    {
                        Name = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Name,
                        Image = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Image,
                        Description = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Description,
                        Type = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).Type,
                        UnitPrice = _amContext.Listing.AsNoTracking().FirstOrDefault(z => z.Id == x.ListingId).UnitPrice,

                    },
                }).First();
        }
    }
}