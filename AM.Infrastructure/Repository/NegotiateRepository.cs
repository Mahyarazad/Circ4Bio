using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Negotiate;
using AM.Domain.NegotiateAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class NegotiateRepository : RepositoryBase<long, Negotiate>, INegotiateRepository
    {
        private readonly AMContext _amContext;
        public NegotiateRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }
        public async Task<NegotiateViewModel> GetNegotiationViewModel(CreateNegotiate Command)
        {
            var negotiate = _amContext.Negotiates
                .Include(x => x.UserNegotiate)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == Command.NegotiateId);

            var sellerId = negotiate.Result.UserNegotiate.Where(x => !x.BuyerBool).First().UserId;
            var buyerId = negotiate.Result.UserNegotiate.Where(x => x.BuyerBool).First().UserId;
            var sellerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == sellerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email
                }).FirstAsync();

            var buyerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == buyerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email
                }).FirstAsync();

            return await _amContext.Listing
                .Select(x => new NegotiateViewModel
                {
                    CreationTime = x.CreationTime,
                    NegotiateId = Command.NegotiateId,
                    ListingId = x.Id,
                    ListingName = x.Name,
                    Currency = x.Currency,
                    ImageString = x.Image,
                    DeliveryMethod = x.DeliveryMethod,
                    Amount = x.Amount,
                    SellerEmail = sellerInfo.Result.Email,
                    BuyerEmail = buyerInfo.Result.Email,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    BuyerId = Command.BuyerId,
                    SellerId = Command.SellerId,
                    IsCanceled = Command.IsCanceled,
                    IsFinished = Command.IsFinished,
                    IsActive = Command.IsActive,
                    ItemType = x.Type,
                    SellerName = sellerInfo.Result.Name,
                    BuyerName = buyerInfo.Result.Name,
                    BuyerImageString = buyerInfo.Result.ImageString,
                    SellerImageString = sellerInfo.Result.ImageString,
                }).AsNoTracking().FirstOrDefaultAsync(x => x.ListingId == Command.ListingId);

        }
        public async Task<NegotiateViewModel> GetNegotiationViewModel(long NegotiateId)
        {
            var negotiate = _amContext.Negotiates
                .Include(x => x.UserNegotiate)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == NegotiateId);

            var sellerId = negotiate.Result.UserNegotiate.Where(x => !x.BuyerBool).First().UserId;
            var buyerId = negotiate.Result.UserNegotiate.Where(x => x.BuyerBool).First().UserId;
            var sellerInfo = await _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == sellerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email

                }).FirstAsync();

            var buyerInfo = await _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == buyerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email
                }).FirstAsync();

            return await _amContext.Listing
                .Select(x => new NegotiateViewModel
                {
                    NegotiateId = NegotiateId,
                    ListingId = x.Id,
                    ListingName = x.Name,
                    Currency = x.Currency,
                    ImageString = x.Image,
                    Amount = x.Amount,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    BuyerId = negotiate.Result.BuyerId,
                    SellerId = negotiate.Result.SellerId,
                    IsCanceled = negotiate.Result.IsCanceled,
                    IsFinished = negotiate.Result.IsFinished,
                    ItemType = x.Type,
                    SellerName = sellerInfo.Name,
                    BuyerName = buyerInfo.Name,
                    BuyerImageString = buyerInfo.ImageString,
                    SellerImageString = sellerInfo.ImageString,
                    BuyerEmail = buyerInfo.Email,
                    SellerEmail = sellerInfo.Email,
                }).AsNoTracking().FirstOrDefaultAsync(x => x.ListingId == negotiate.Result.ListingId);
        }
        public async Task<List<CreateNegotiate>> AllListingItemsBuyyer(long BuyerId)
        {
            return await _amContext.Negotiates
                .Where(x => x.BuyerId == BuyerId)
                .Select(x => new CreateNegotiate
                {
                    NegotiateId = x.Id,
                    ListingId = x.ListingId,
                    BuyerId = x.BuyerId,
                    SellerId = x.SellerId,
                    IsCanceled = x.IsCanceled,
                    IsFinished = x.IsFinished,
                    IsActive = x.IsActive,
                }).AsNoTracking().OrderByDescending(x => x.NegotiateId).ToListAsync();
        }
        public async Task<List<CreateNegotiate>> AllListingItemsSeller(long SellerId)
        {
            return await _amContext.Negotiates
                .Where(x => x.SellerId == SellerId)
                .Select(x => new CreateNegotiate
                {
                    NegotiateId = x.Id,
                    ListingId = x.ListingId,
                    BuyerId = x.BuyerId,
                    SellerId = x.SellerId,
                    IsCanceled = x.IsCanceled,
                    IsFinished = x.IsFinished,
                    IsActive = x.IsActive
                }).AsNoTracking().OrderByDescending(x => x.NegotiateId).ToListAsync();
        }
        public async Task<List<MessageViewModel>> GetMessages(long NegotiateId)
        {
            var query = _amContext.Negotiates
                .Include(x => x.UserNegotiate)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefaultAsync(x => x.Id == NegotiateId);

            var sellerId = query.Result.UserNegotiate.Where(x => !x.BuyerBool).First().UserId;
            var buyerId = query.Result.UserNegotiate.Where(x => x.BuyerBool).First().UserId;
            var sellerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == sellerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Letter = x.Email.Substring(0, 1)
                }).FirstAsync();

            var buyerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == buyerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Letter = x.Email.Substring(0, 1)
                }).FirstAsync();

            var result = query.Result.Messages.Select(x => new MessageViewModel
            {
                MessageId = x.Id,
                UserId = x.UserId,
                UserEntity = x.UserEntity,
                NegotiateId = x.NegotiateId,
                MessageBody = x.MessageBody,
                FileString = x.FilePathString,
                CreationTime = x.CreationTime,
                BuyerId = buyerId,
                SellerId = sellerId,
                BuyyerImageString = buyerInfo.Result.ImageString,
                SellerImageString = sellerInfo.Result.ImageString,
                BuyyerLetter = buyerInfo.Result.Letter,
                SellerLetter = sellerInfo.Result.Letter

            }).OrderBy(x => x.MessageId).ToList();

            return await Task.FromResult(result);
        }
        public async Task<OperationResult> ActiveNegotiation(long NegotiateId)
        {
            var result = new OperationResult();
            var target = await _amContext.Negotiates.FirstOrDefaultAsync(x => x.Id == NegotiateId);
            target.Activate();
            _amContext.SaveChanges();

            return result.Succeeded();
        }
    }
}