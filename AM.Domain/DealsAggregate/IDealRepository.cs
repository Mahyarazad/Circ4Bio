using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Domain;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;

namespace AM.Domain
{
    public interface IDealRepository : IRepository<long, Deal>
    {
        Task<List<DealViewModel>> GetAllDeals(long UserId);
        DealViewModel GetDealWithDealId(long DealId);
    }
}