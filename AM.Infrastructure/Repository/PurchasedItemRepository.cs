using _0_Framework.Infrastructure;
using AM.Domain.Supplied.PurchasedAggregate;

namespace AM.Infrastructure.Repository
{
    public class PurchasedItemRepository : RepositoryBase<long, PurchasedItem>, IPurchasedItemRepository
    {
        private readonly AMContext _amContext;
        public PurchasedItemRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }
    }
}