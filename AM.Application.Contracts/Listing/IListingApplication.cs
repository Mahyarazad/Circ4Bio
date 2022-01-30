using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;

namespace AM.Application.Contracts.Listing
{
    public interface IListingApplication
    {
        OperationResult Create(CreateListing command);
        OperationResult Edit(EditListing command);
        List<ListingViewModel> GetAllListing();
        List<ListingViewModel> GetAllPublicListing();
        List<ListingViewModel> GetUserListing(long id);
        ListingViewModel GetDetailListing(long id);
        long GetOwnerUserID(long id);
        List<ListingViewModel> GetDeletedUserListing(long id);
        EditListing GetEditListing(long listingId);
        OperationResult MarkPrivate(long id);
        OperationResult MarkPublic(long id);
        OperationResult Delete(long id);
        OperationResult IncrementAmount(InputAmount command);
        OperationResult DeccrementAmount(InputAmount command);
        List<ListingOperationLog> GetListingOperationLog(long Id);
        List<ActiveListing> GetActiveListing(long userId);

    }
}