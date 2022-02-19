using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;

namespace AM.Application.Contracts.Listing
{
    public interface IListingApplication
    {
        Task<OperationResult> Create(CreateListing command);
        Task<OperationResult> Edit(EditListing command);
        Task<List<ListingViewModel>> GetAllListing();
        Task<List<ListingViewModel>> GetAllPublicListing();
        Task<List<ListingViewModel>> GetUserListing(long id);
        Task<ListingViewModel> GetDetailListing(long id);
        Task<long> GetOwnerUserID(long id);
        Task<List<ListingViewModel>> GetDeletedUserListing(long id);
        Task<EditListing> GetEditListing(long listingId);
        Task<OperationResult> MarkPrivate(long id);
        Task<OperationResult> MarkPublic(long id);
        Task<OperationResult> Delete(long id);
        Task<OperationResult> IncrementAmount(InputAmount command);
        Task<OperationResult> DeccrementAmount(InputAmount command);
        Task<List<ListingOperationLog>> GetListingOperationLog(long Id);
        Task<List<ActiveListing>> GetActiveListing(long userId);

    }
}