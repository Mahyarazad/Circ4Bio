using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;

namespace AM.Domain.ListingAggregate
{
    public interface IListingRepository : IRepository<long, Listing>
    {
        List<ListingViewModel> GetAllListing();
        List<ListingViewModel> GetAllPublicListing();
        ListingViewModel GetDetailListing(long Id);
        List<ListingViewModel> GetUserListing(long Id);
        List<ListingViewModel> GetDeletedUserListing(long Id);
        EditListing GetListingDetail(long ListingId);
        long GetOwnerUserID(long Id);
        List<ActiveListing> GetActiveListing(long userId);

    }
}