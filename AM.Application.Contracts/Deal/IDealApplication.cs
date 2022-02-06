using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Deal
{
    public interface IDealApplication
    {
        OperationResult CreateDeal(CreateDeal Command);
        OperationResult EditDeal(EditDeal Command);
        OperationResult RejectDeal(long Id);
        OperationResult FinishDeal(long Id);
        OperationResult AtivateDeal(long Id);
        OperationResult PaymentReceived(long Id);
        List<DealViewModel> GetAllDeals(long UserId);
        DealViewModel GetDealWithDealId(long DealId);

    }
}