using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Negotiate
{
    public interface INegotiateApplication
    {
        Task<OperationResult> Create(CreateNegotiate Command);
        Task<OperationResult> SendMessage(NewMessage Command);
        Task<OperationResult> CancelNegotiation(CreateNegotiate Command);
        NegotiateViewModel GetNegotiationViewModel(CreateNegotiate Command);
        NegotiateViewModel GetNegotiationViewModel(long NegotiateId);
        List<CreateNegotiate> AllListingItemsBuyyer(long BuyyerId);
        List<CreateNegotiate> AllListingItemsSeller(long SellerId);
        List<CreateNegotiate> GetAllListingNegotiation();
        List<MessageViewModel> GetMessages(long NegotiateId);
        Task<OperationResult> ActiveNegotiation(long NegotiateId);
    }
}