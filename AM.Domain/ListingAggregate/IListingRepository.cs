using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Domain;
using AM.Application.Contracts.Listing;

namespace AM.Domain.ListingAggregate
{
    public interface IListingRepository : IRepository<long, Listing>
    {
        Task<List<ListingViewModel>> GetAllListing();
        Task<List<ListingViewModel>> GetAllListingForAdmin();
        Task<ListingViewModel> GetDetailListing(long ListingId);
        Task<List<ListingViewModel>> GetUserListing(long Id);
        Task<List<ListingViewModel>> GetDeletedUserListing(long Id);
        Task<EditListing> GetListingDetail(long ListingId);
        Task<long> GetOwnerUserID(long Id);
        Task<List<ActiveListing>> GetActiveListing(long userId);
        long GetTheLastListingId();
    }
}