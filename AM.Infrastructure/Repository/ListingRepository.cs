using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<List<ListingViewModel>> GetAllListing()
        {
            var query = await _amContext.Listing
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
                    IsService = x.IsService,
                    Currency = x.Currency
                }).AsNoTracking()
                .OrderByDescending(x => x.Id).ToListAsync();

            return query;
        }
        public async Task<List<ListingViewModel>> GetAllPublicListing()
        {
            var query = await _amContext.Listing
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
                .OrderByDescending(x => x.Id).ToListAsync();
            return query;
        }
        public async Task<List<ListingViewModel>> GetUserListing(long Id)
        {
            var query = await _amContext.Listing
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
                .OrderByDescending(x => x.Id).ToListAsync();

            return query;
        }
        public Task<ListingViewModel> GetDetailListing(long Id)
        {
            var purchasedQuery =
                _amContext.PurchasedItems
                .Where(x => x.ListingId == Id)
                .AsSingleQuery()
                .AsNoTracking();

            var query = _amContext.Listing
                .Include(x => x.User)
                .Include(x => x.DealList)
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
                    Currency = x.Currency,
                    PurchaseCount = purchasedQuery.Count()
                }).AsNoTracking().ToList().Last();

            return Task.FromResult(query);
        }
        public async Task<List<ListingViewModel>> GetDeletedUserListing(long Id)
        {
            var query = await _amContext.Listing
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
                .OrderByDescending(x => x.Id).ToListAsync();

            return query;
        }
        public async Task<EditListing> GetListingDetail(long ListingId)
        {
            return await _amContext.Listing
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

                }).AsNoTracking().FirstAsync();
        }
        public Task<long> GetOwnerUserID(long Id)
        {
            return Task.FromResult(_amContext.Listing.FirstOrDefaultAsync(x => x.Id == Id).Result.UserId);
        }
        public async Task<List<ActiveListing>> GetActiveListing(long userId)
        {
            return await _amContext.Listing
                .Where(x => x.UserId == userId & !x.IsDeleted)
                .Select(x => new ActiveListing
                {
                    Id = x.Id
                }).AsNoTracking().ToListAsync();
        }

        public long GetTheLastListingId()
        {
            return _amContext.Listing.OrderByDescending(x => x.Id).FirstOrDefault().Id;
        }
    }
}