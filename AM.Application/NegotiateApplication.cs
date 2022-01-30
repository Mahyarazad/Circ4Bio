using System.Collections.Generic;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Negotiate;
using AM.Domain.ListingAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.UserAggregate;

namespace AM.Application
{
    public class NegotiateApplication : INegotiateApplication
    {
        private readonly IUserRepository _userRepository;
        private readonly IListingRepository _listingRepository;
        private readonly INegotiateRepository _negotiateRepository;

        public NegotiateApplication(INegotiateRepository negotiateRepository,
            IListingRepository listingRepository,
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _listingRepository = listingRepository;
            _negotiateRepository = negotiateRepository;
        }

        public OperationResult Create(CreateNegotiate Command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == Command.ListingId))
                return result.Failed(ApplicationMessage.RecordNotFound);
            _negotiateRepository.Create(new Negotiate(Command.ListingId, Command.BuyyerId, Command.SellerId));
            _negotiateRepository.SaveChanges();
            return result.Succeeded();

        }

        public OperationResult SendMessage(NewMessage Command)
        {
            var result = new OperationResult();
            if (!_listingRepository.Exist(x => x.Id == Command.NegotiateId))
                return result.Failed(ApplicationMessage.RecordNotFound);
            Command.UserEntity = _userRepository.Get(Command.UserId).UserName;
            var targetNegotiation = _negotiateRepository.Get(Command.NegotiateId);
            targetNegotiation.AddMessage(Command.MessageBody, Command.UserId, Command.UserEntity.Substring(0, 1));
            _negotiateRepository.SaveChanges();
            return result.Succeeded();

        }

        public NegotiateViewModel GetNegotiationList(CreateNegotiate Command)
        {
            return _negotiateRepository.GetNegotiationList(Command);
        }

        public List<CreateNegotiate> AllListingItems(long BuyyerId)
        {
            return _negotiateRepository.AllListingItems(BuyyerId);
        }
    }
}