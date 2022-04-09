﻿using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Negotiate;

namespace AM.Domain.NegotiateAggregate
{
    public interface INegotiateRepository : IRepository<long, Negotiate>
    {
        NegotiateViewModel GetNegotiationViewModel(CreateNegotiate Command);
        NegotiateViewModel GetNegotiationViewModel(long NegotiateId);
        List<CreateNegotiate> AllListingItemsBuyyer(long BuyerId);
        List<CreateNegotiate> AllListingItemsSeller(long SellerId);
        List<CreateNegotiate> GetAllListingNegotiation();
        List<MessageViewModel> GetMessages(long NegotiateId);
        Task<OperationResult> ActiveNegotiation(long NegotiateId);

    }
}