using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Deal;

namespace AM.Domain.Supplied.PurchasedAggregate
{
    public interface IPurchasedItemRepository : IRepository<long, PurchasedItem>
    {
        List<PurchasedItemModel> GetPurchasedList(long UserId);
    }
}