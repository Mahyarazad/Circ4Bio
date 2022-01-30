using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Negotiate
{
    public interface INegotiateApplication
    {
        OperationResult Create(CreateNegotiate Command);
        OperationResult SendMessage(NewMessage Command);
        NegotiateViewModel GetNegotiationList(CreateNegotiate Command);
        List<CreateNegotiate> AllListingItems(long BuyyerId);
    }
}