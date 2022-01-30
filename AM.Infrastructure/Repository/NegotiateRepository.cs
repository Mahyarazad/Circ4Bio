using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Negotiate;
using AM.Domain.NegotiateAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class NegotiateRepository : RepositoryBase<long, Negotiate>, INegotiateRepository
    {
        private readonly AMContext _amContext;
        public NegotiateRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }
        public NegotiateViewModel GetNegotiationList(CreateNegotiate Command)
        {
            return _amContext.Listing
                .Select(x => new NegotiateViewModel
                {
                    ListingId = x.Id,
                    ListingName = x.Name,
                    Currency = x.Currency,
                    ImageString = x.Image,
                    Amount = x.Amount,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    BuyyerId = Command.BuyyerId,
                    SellerId = Command.SellerId,
                    logId = x.Id,
                    BuyyerImageString = _amContext.Users
                        .AsNoTracking().FirstOrDefault(x => x.Id == Command.BuyyerId).Avatar,
                    SellerImageString = _amContext.Users
                        .AsNoTracking().FirstOrDefault(x => x.Id == Command.SellerId).Avatar,
                }).AsNoTracking().FirstOrDefault(x => x.ListingId == Command.ListingId);

        }

        public List<CreateNegotiate> AllListingItems(long BuyyerId)
        {
            return _amContext.Negotiates
                .Where(x => x.BuyyerId == BuyyerId)
                .Select(x => new CreateNegotiate
                {
                    logId = x.Id,
                    ListingId = x.ListingId,
                    BuyyerId = x.BuyyerId,
                    SellerId = x.SellerId
                }).AsNoTracking().OrderByDescending(x => x.logId).ToList();
        }
    }
}