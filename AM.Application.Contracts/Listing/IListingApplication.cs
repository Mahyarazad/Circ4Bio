using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Listing
{
    public interface IListingApplication
    {
        OperationResult Create(CreateListing command);
        OperationResult Edit(EditListing command);
        List<ListingViewModel> GetAllListing();
        List<ListingViewModel> GetUserListing(long id);
        List<ListingViewModel> GetDeletedUserListing(long id);
        EditListing GetEditListing(long listingId);
        OperationResult MarkPrivate(long id);
        OperationResult MarkPublic(long id);
        OperationResult Delete(long id);
    }
}