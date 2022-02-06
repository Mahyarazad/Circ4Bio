using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;

namespace AM.Domain
{
    public interface IDealRepository : IRepository<long, Deal>
    {
        List<DealViewModel> GetAllDeals(long UserId);
        DealViewModel GetDealWithDealId(long DealId);
    }
}