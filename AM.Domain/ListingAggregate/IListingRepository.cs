using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Listing;

namespace AM.Domain.ListingAggregate
{
    public interface IListingRepository : IRepository<long, Listing>
    {
        List<ListingViewModel> GetAllListing();
        List<ListingViewModel> GetUserListing(long Id);
        List<ListingViewModel> GetDeletedUserListing(long Id);
        EditListing GetListingDetail(long ListingId);

    }
}