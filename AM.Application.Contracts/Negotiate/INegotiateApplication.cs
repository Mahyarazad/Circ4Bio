using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Negotiate
{
    public interface INegotiateApplication
    {
        OperationResult Create(CreateNegotiate Command);
        OperationResult SendMessage(NewMessage Command);
        OperationResult CancelNegotiation(CreateNegotiate Command);
        NegotiateViewModel GetNegotiationViewModel(CreateNegotiate Command);
        NegotiateViewModel GetNegotiationViewModel(long NegotiateId);
        List<CreateNegotiate> AllListingItemsBuyyer(long BuyyerId);
        List<CreateNegotiate> AllListingItemsSeller(long SellerId);
        List<MessageViewModel> GetMessages(long NegotiateId);
    }
}