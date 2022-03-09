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
        public NegotiateViewModel GetNegotiationViewModel(CreateNegotiate Command)
        {
            var negotiate = _amContext.Negotiates
                .Include(x => x.UserNegotiate)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == Command.NegotiateId);

            var sellerId = negotiate.UserNegotiate
                .Where(x => !x.BuyerBool)
                .First().UserId;
            var buyerId = negotiate.UserNegotiate
                .Where(x => x.BuyerBool)
                .First().UserId;

            var sellerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == sellerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email
                }).First();

            var buyerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == buyerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email
                }).First();

            return _amContext.Listing.AsSingleQuery()
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
                    SellerEmail = sellerInfo.Email,
                    BuyerEmail = buyerInfo.Email,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    BuyerId = Command.BuyerId,
                    SellerId = Command.SellerId,
                    IsCanceled = Command.IsCanceled,
                    IsFinished = Command.IsFinished,
                    IsActive = Command.IsActive,
                    QuatationSent = negotiate.QuatationSent,
                    IsRejected = negotiate.IsRejected,
                    ItemType = x.Type,
                    SellerName = sellerInfo.Name,
                    BuyerName = buyerInfo.Name,
                    BuyerImageString = buyerInfo.ImageString,
                    SellerImageString = sellerInfo.ImageString,
                }).AsNoTracking().FirstOrDefault(x => x.ListingId == Command.ListingId);

        }
        public NegotiateViewModel GetNegotiationViewModel(long NegotiateId)
        {
            var negotiate = _amContext.Negotiates
                .Include(x => x.UserNegotiate)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == NegotiateId);

            var sellerId = negotiate.UserNegotiate.Where(x => !x.BuyerBool).First().UserId;
            var buyerId = negotiate.UserNegotiate.Where(x => x.BuyerBool).First().UserId;
            var sellerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == sellerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email

                }).First();

            var buyerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == buyerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Name = $"{x.FirstName} {x.LastName}",
                    Email = x.Email
                }).First();

            return _amContext.Listing
                .Select(x => new NegotiateViewModel
                {
                    NegotiateId = NegotiateId,
                    ListingId = x.Id,
                    ListingName = x.Name,
                    Currency = x.Currency,
                    ImageString = x.Image,
                    Amount = x.Amount,
                    Unit = x.Unit,
                    CreationTime = x.CreationTime,
                    UnitPrice = x.UnitPrice,
                    BuyerId = negotiate.BuyerId,
                    SellerId = negotiate.SellerId,
                    IsCanceled = negotiate.IsCanceled,
                    IsFinished = negotiate.IsFinished,
                    ItemType = x.Type,
                    QuatationSent = negotiate.QuatationSent,
                    SellerName = sellerInfo.Name,
                    BuyerName = buyerInfo.Name,
                    BuyerImageString = buyerInfo.ImageString,
                    SellerImageString = sellerInfo.ImageString,
                    BuyerEmail = buyerInfo.Email,
                    SellerEmail = sellerInfo.Email,
                }).AsNoTracking().FirstOrDefault(x => x.ListingId == negotiate.ListingId);
        }
        public List<CreateNegotiate> AllListingItemsBuyyer(long BuyerId)
        {
            return _amContext.Negotiates
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
                }).AsNoTracking().OrderByDescending(x => x.NegotiateId).ToList();
        }
        public List<CreateNegotiate> AllListingItemsSeller(long SellerId)
        {
            return _amContext.Negotiates
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
                }).AsNoTracking().OrderByDescending(x => x.NegotiateId).ToList();
        }
        public List<MessageViewModel> GetMessages(long NegotiateId)
        {
            var query = _amContext.Negotiates
                .Include(x => x.UserNegotiate)
                .AsNoTracking()
                .AsSingleQuery()
                .FirstOrDefault(x => x.Id == NegotiateId);

            var sellerId = query.UserNegotiate.Where(x => !x.BuyerBool).First().UserId;
            var buyerId = query.UserNegotiate.Where(x => x.BuyerBool).First().UserId;
            var sellerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == sellerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Letter = x.Email.Substring(0, 1)
                }).First();

            var buyerInfo = _amContext.Users
                .AsNoTracking()
                .AsSingleQuery()
                .Where(x => x.Id == buyerId)
                .Select(x => new UserInfoMessaging
                {
                    ImageString = x.Avatar,
                    Letter = x.Email.Substring(0, 1)
                }).First();

            var result = query.Messages.Select(x => new MessageViewModel
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
                BuyyerImageString = buyerInfo.ImageString,
                SellerImageString = sellerInfo.ImageString,
                BuyyerLetter = buyerInfo.Letter,
                SellerLetter = sellerInfo.Letter

            }).OrderBy(x => x.MessageId).ToList();

            return result;
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