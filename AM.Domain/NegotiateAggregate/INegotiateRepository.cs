using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Negotiate;

namespace AM.Domain.NegotiateAggregate
{
    public interface INegotiateRepository : IRepository<long, Negotiate>
    {
        Task<NegotiateViewModel> GetNegotiationViewModel(CreateNegotiate Command);
        Task<NegotiateViewModel> GetNegotiationViewModel(long NegotiateId);
        Task<List<CreateNegotiate>> AllListingItemsBuyyer(long BuyerId);
        Task<List<CreateNegotiate>> AllListingItemsSeller(long SellerId);
        Task<List<MessageViewModel>> GetMessages(long NegotiateId);
        Task<OperationResult> ActiveNegotiation(long NegotiateId);

    }
}