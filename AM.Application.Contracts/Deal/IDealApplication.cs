using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Deal
{
    public interface IDealApplication
    {
        Task<OperationResult> CreateQuatation(CreateDeal Command);
        Task<OperationResult> EditDeal(EditDeal Command);
        Task<OperationResult> RejectDeal(long Id);
        Task<OperationResult> FinishDeal(long Id);
        Task<OperationResult> AtivateDeal(long Id);
        Task<OperationResult> PaymentReceived(long Id);
        Task<List<DealViewModel>> GetAllDeals(long UserId);
        Task<DealViewModel> GetDealWithDealId(long DealId);

    }
}