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
        Task<NegotiateViewModel> GetNegotiationViewModel(CreateNegotiate Command);
        Task<NegotiateViewModel> GetNegotiationViewModel(long NegotiateId);
        Task<List<CreateNegotiate>> AllListingItemsBuyyer(long BuyyerId);
        Task<List<CreateNegotiate>> AllListingItemsSeller(long SellerId);
        Task<List<MessageViewModel>> GetMessages(long NegotiateId);
        Task<OperationResult> ActiveNegotiation(long NegotiateId);
    }
}