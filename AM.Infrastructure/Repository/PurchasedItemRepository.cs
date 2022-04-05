using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Deal;
using AM.Domain.Supplied.PurchasedAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class PurchasedItemRepository : RepositoryBase<long, PurchasedItem>, IPurchasedItemRepository
    {
        private readonly AMContext _amContext;
        public PurchasedItemRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<PurchasedItemModel> GetPurchasedList(long UserId)
        {
            return _amContext.PurchasedItems
                .Where(x => x.UserId == UserId)
                .AsNoTracking()
                .Select(x => new PurchasedItemModel
                {
                    UserId = x.UserId,
                    Amount = x.Amount,
                    Currency = x.Currency,
                    ListingId = x.ListingId,
                    TotalPaid = x.TotalPaid,
                    UnitPrice = x.UnitPrice
                }).ToList();
        }
    }
}