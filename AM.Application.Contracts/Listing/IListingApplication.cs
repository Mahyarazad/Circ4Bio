using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Listing
{
    public interface IListingApplication
    {
        OperationResult Create(CreateListing command);
        OperationResult Edit(EditListing command);
        List<ListingViewModel> GetUserListing(long id);
        EditListing GetEditListing(long listingId);
    }
}