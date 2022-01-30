using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Negotiate;

namespace AM.Domain.NegotiateAggregate
{
    public interface INegotiateRepository : IRepository<long, Negotiate>
    {
        NegotiateViewModel GetNegotiationList(CreateNegotiate Command);
        List<CreateNegotiate> AllListingItems(long BuyyerId);
    }
}