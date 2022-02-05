using System.Collections.Generic;
using System.Linq;
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
            return _amContext.Listing
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
                    SellerEmail = _amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == Command.SellerId).Email,
                    BuyerEmail = _amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == Command.BuyerId).Email,
                    Unit = x.Unit,
                    UnitPrice = x.UnitPrice,
                    BuyerId = Command.BuyerId,
                    SellerId = Command.SellerId,
                    IsCanceled = Command.IsCanceled,
                    IsFinished = Command.IsFinished,
                    ItemType = x.Type,
                    SellerName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == Command.SellerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == Command.SellerId).LastName}",
                    BuyerName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == Command.BuyerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == Command.BuyerId).LastName}",
                    BuyerImageString = _amContext.Users
                        .AsNoTracking().FirstOrDefault(x => x.Id == Command.BuyerId).Avatar,
                    SellerImageString = _amContext.Users
                        .AsNoTracking().FirstOrDefault(x => x.Id == Command.SellerId).Avatar,
                }).AsNoTracking().FirstOrDefault(x => x.ListingId == Command.ListingId);

        }
        public NegotiateViewModel GetNegotiationViewModel(long NegotiateId)
        {
            var negotiate = _amContext.Negotiates
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == NegotiateId);
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
                    UnitPrice = x.UnitPrice,
                    BuyerId = negotiate.BuyerId,
                    SellerId = negotiate.SellerId,
                    IsCanceled = negotiate.IsCanceled,
                    IsFinished = negotiate.IsFinished,
                    ItemType = x.Type,
                    SellerName = $"{_amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == negotiate.BuyerId).FirstName} {_amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id == negotiate.BuyerId).LastName}",
                    BuyerImageString = _amContext.Users
                        .AsNoTracking().FirstOrDefault(x => x.Id == negotiate.BuyerId).Avatar,
                    SellerImageString = _amContext.Users
                        .AsNoTracking().FirstOrDefault(x => x.Id == negotiate.SellerId).Avatar,
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
                    IsFinished = x.IsFinished
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
                    IsFinished = x.IsFinished
                }).AsNoTracking().OrderByDescending(x => x.NegotiateId).ToList();
        }
        public List<MessageViewModel> GetMessages(long NegotiateId)
        {
            var query = _amContext.Negotiates
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == NegotiateId).Messages;
            return query.Select(x => new MessageViewModel
            {
                MessageId = x.Id,
                UserId = x.UserId,
                UserEntity = x.UserEntity,
                NegotiateId = x.NegotiateId,
                MessageBody = x.MessageBody,
                FileString = x.FilePathString,
                CreationTime = x.CreationTime,
                BuyerId = _amContext.Negotiates.AsNoTracking().FirstOrDefault(x => x.Id == NegotiateId).BuyerId,
                SellerId = _amContext.Negotiates.AsNoTracking().FirstOrDefault(x => x.Id == NegotiateId).SellerId,
                BuyyerImageString = _amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id ==
                    _amContext.Negotiates.AsNoTracking().FirstOrDefault(x => x.Id == NegotiateId).BuyerId).Avatar,
                SellerImageString = _amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id ==
                    _amContext.Negotiates.AsNoTracking().FirstOrDefault(x => x.Id == NegotiateId).SellerId).Avatar,
                BuyyerLetter = _amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id ==
                    _amContext.Negotiates.AsNoTracking().FirstOrDefault(x => x.Id == NegotiateId).BuyerId).Email.Substring(0, 1),
                SellerLetter = _amContext.Users.AsNoTracking().FirstOrDefault(x => x.Id ==
                    _amContext.Negotiates.AsNoTracking().FirstOrDefault(x => x.Id == NegotiateId).SellerId).Email.Substring(0, 1)

            }).OrderBy(x => x.MessageId).ToList();
        }
    }
}