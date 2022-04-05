using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Deal;
using AM.Domain.Supplied.PurchasedAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class SuppliedItemRepository : RepositoryBase<long, SuppliedItem>, ISuppliedItemRepository
    {
        private readonly AMContext _amContext;
        public SuppliedItemRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<SuppliedItemModel> GetSuppliedList(long UserId)
        {
            return _amContext.SuppliedItems
                .Where(x => x.UserId == UserId)
                .AsNoTracking()
                .Select(x => new SuppliedItemModel
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