using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Listing;
using AM.Domain.ListingAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class ListingRepository : RepositoryBase<long, Listing>, IListingRepository
    {
        private readonly AMContext _amContext;
        public ListingRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<ListingViewModel> GetUserListing(long id)
        {
            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
                .Include(x => x.SupplyList)
                .Include(x => x.PurchaseList)
                .Where(x => x.UserId == id)
                .Select(x => new ListingViewModel
                {
                    Amount = x.Amount,
                    CreationTime = x.CreationTime,
                    DeliveryMethod = x.DeliveryMethod,
                    Description = x.Description,
                    Id = x.Id,
                    Name = x.Name,
                    FullName = x.User.FirstName + x.User.LastName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Type = x.Type,
                    Image = x.Image,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    Status = x.Status,
                    IsDeleted = x.IsDeleted,
                })
                .OrderByDescending(x => x.Id).ToList();

            return query;
        }

        public EditListing GetListingDetail(long ListingId)
        {
            return _amContext.Listing
                .Where(x => x.Id == ListingId)
                .Select(x => new EditListing
                {
                    Amount = x.Amount,
                    Description = x.Description,
                    DeliveryMethod = x.DeliveryMethod,
                    Name = x.Name,
                    Unit = x.Unit,
                    Status = x.Status,
                    ImageString = x.Image,
                    UnitPrice = x.UnitPrice,
                    Id = x.Id
                }).First();
        }
    }
}