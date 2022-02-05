using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
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
        public List<ListingViewModel> GetAllListing()
        {
            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
                .Include(x => x.SupplyList)
                .Include(x => x.PurchaseList)
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
                    PublicStatus = x.Status,
                    IsDeleted = x.IsDeleted,
                    Currency = x.Currency
                }).AsNoTracking()
                .OrderByDescending(x => x.Id).ToList();

            return query;
        }
        public List<ListingViewModel> GetAllPublicListing()
        {
            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
                .Include(x => x.SupplyList)
                .Include(x => x.PurchaseList)
                .Where(x => !x.Status & !x.IsDeleted)
                .Select(x => new ListingViewModel
                {
                    Amount = x.Amount,
                    CreationTime = x.CreationTime,
                    DeliveryMethod = x.DeliveryMethod,
                    Description = x.Description,
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    FullName = x.User.FirstName + x.User.LastName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Type = x.Type,
                    Image = x.Image,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    PublicStatus = x.Status,
                    IsDeleted = x.IsDeleted,
                    Currency = x.Currency
                }).AsNoTracking()
                .OrderByDescending(x => x.Id).ToList();
            return query;
        }
        public List<ListingViewModel> GetUserListing(long Id)
        {
            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
                .Include(x => x.SupplyList)
                .Include(x => x.PurchaseList)
                .Where(x => x.UserId == Id)
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
                    PublicStatus = x.Status,
                    IsDeleted = x.IsDeleted,
                    IsService = x.IsService,
                    Currency = x.Currency
                }).AsNoTracking()
                .OrderByDescending(x => x.Id).ToList();

            return query;
        }
        public ListingViewModel GetDetailListing(long Id)
        {
            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
                .Include(x => x.SupplyList)
                .Include(x => x.PurchaseList)
                .Where(x => x.Id == Id)
                .Select(x => new ListingViewModel
                {
                    Amount = x.Amount,
                    CreationTime = x.CreationTime,
                    DeliveryMethod = x.DeliveryMethod,
                    Description = x.Description,
                    Id = x.Id,
                    UserId = x.UserId,
                    Name = x.Name,
                    FullName = x.User.FirstName + x.User.LastName,
                    Email = x.User.Email,
                    PhoneNumber = x.User.PhoneNumber,
                    Type = x.Type,
                    Image = x.Image,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    PublicStatus = x.Status,
                    IsService = x.IsService,
                    Currency = x.Currency
                }).AsNoTracking().ToList().Last();

            return query;
        }
        public List<ListingViewModel> GetDeletedUserListing(long Id)
        {
            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
                .Include(x => x.SupplyList)
                .Include(x => x.PurchaseList)
                .Where(x => x.UserId == Id && x.IsDeleted)
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
                    PublicStatus = x.Status,
                    IsDeleted = x.IsDeleted,
                    Currency = x.Currency
                }).AsNoTracking()
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
                    Type = x.Type,
                    ImageString = x.Image,
                    UnitPrice = x.UnitPrice,
                    Id = x.Id,
                    OwnerUserId = x.UserId,
                    IsService = x.IsService,
                    Currency = x.Currency

                }).AsNoTracking().First();
        }
        public long GetOwnerUserID(long Id)
        {
            return _amContext.Listing.FirstOrDefault(x => x.Id == Id).UserId;
        }
        public List<ActiveListing> GetActiveListing(long userId)
        {
            return _amContext.Listing
                .Where(x => x.UserId == userId & !x.IsDeleted)
                .Select(x => new ActiveListing
                {
                    Id = x.Id
                }).AsNoTracking().ToList();
        }
    }
}