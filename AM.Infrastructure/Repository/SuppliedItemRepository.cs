using _0_Framework.Infrastructure;
using AM.Domain.Supplied.PurchasedAggregate;

namespace AM.Infrastructure.Repository
{
    public class SuppliedItemRepository : RepositoryBase<long, SuppliedItem>, ISuppliedItemRepository
    {
        private readonly AMContext _amContext;
        public SuppliedItemRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }
    }
}