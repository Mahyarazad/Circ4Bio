using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                    IsDeleted = x.IsDeleted,
                    IsActive = x.IsActive,
                    IsFinished = x.IsFinished,
                    IsRejected = x.IsRejected,
                    QuatationSent = x.QuatationSent,
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

        public async Task<DealViewModel> GetDealWithDealId(long DealId)
        {
            var deal = _amContext.Deals
                .Include(x => x.Listing)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.NegotiateId == DealId);

            var query = new DealViewModel
            {
                DealId = deal.Result.Id,
                Currency = deal.Result.Currency,
                NegotiateId = deal.Result.NegotiateId,
                Amount = deal.Result.Amount,
                IsDeleted = deal.Result.IsDeleted,
                IsActive = deal.Result.IsActive,
                IsFinished = deal.Result.IsFinished,
                Location = deal.Result.Location,
                ListingId = deal.Result.ListingId,
                TotalCost = deal.Result.TotalCost,
                DeliveryCost = deal.Result.DeliveryCost,
                DeliveryMethod = deal.Result.DeliveryMethod,
                ContractFileString = deal.Result.ContractFile,
                TrackingCode = deal.Result.TrackingCode,
                Buyer = new UserViewModel
                {
                    Id = deal.Result.BuyerId,
                    FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.BuyerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.BuyerId).LastName}",
                    Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.BuyerId).Avatar,
                    Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.BuyerId).Email
                },
                Seller = new UserViewModel
                {
                    Id = deal.Result.SellerId,
                    FullName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.SellerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.SellerId).LastName}",
                    Avatar = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.SellerId).Avatar,
                    Email = _amContext.Users.AsNoTracking().FirstOrDefault(z => z.Id == deal.Result.SellerId).Email
                },
                Unit = deal.Result.Unit,
                Listing = new ListingViewModel
                {
                    Name = deal.Result.Listing.Name,
                    Image = deal.Result.Listing.Image,
                    Type = deal.Result.Listing.Type,
                    UnitPrice = deal.Result.Listing.UnitPrice,
                    Description = deal.Result.Listing.Description,

                }
            };

            return query;
        }
    }
}