using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.Listing;

namespace AM.Domain
{
    public interface IDealRepository : IRepository<long, Deal>
    {
        Task<List<DealViewModel>> GetAllDeals(long UserId);
        Task<List<DealViewModel>> GetAllDeals();
        Task<List<DealViewModel>> GetAllFinishedDeals(long UserId);
        DealViewModel GetDealWithNegotiateId(long NegotiateId);
        DealViewModel GetDealWithDealId(long DealId);
        DealViewModel GetDealWithDealIdforDealIndex(long DealId);
        DealViewModel ReturnDealIdWithTrackingRef(string TrackingCode);
    }
}