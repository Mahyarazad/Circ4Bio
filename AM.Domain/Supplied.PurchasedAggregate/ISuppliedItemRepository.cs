using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Deal;

namespace AM.Domain.Supplied.PurchasedAggregate
{
    public interface ISuppliedItemRepository : IRepository<long, SuppliedItem>
    {
        List<SuppliedItemModel> GetSuppliedList(long UserId);
    }
}