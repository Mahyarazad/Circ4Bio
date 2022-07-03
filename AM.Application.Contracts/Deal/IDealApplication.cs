using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Deal
{
    public interface IDealApplication
    {
        Task<OperationResult> CreateQuotation(CreateDeal Command);
        Task<OperationResult> EditDeal(DealViewModel Command);
        Task<OperationResult> RejectDeal(long Id);
        Task<OperationResult> FinishDeal(DealViewModel Command);
        Task<OperationResult> AtivateDeal(DealViewModel Command);
        Task<OperationResult> PaymentReceived(long Id);
        Task<List<DealViewModel>> GetAllDeals(long UserId);
        Task<List<DealViewModel>> GetAllDeals();
        Task<List<DealViewModel>> GetAllFinishedDeals(long UserId);
        DealViewModel GetDealWithNegotiateId(long NegotiateId);
        DealViewModel GetDealWithDealId(long DealId);
        DealViewModel GetDealWithDealIdforDealIndex(long DealId);
        DealViewModel ReturnDealIdWithTrackingRef(string TrackingCode);
        Task<OperationResult> SuppliedItem(SuppliedItemModel Command);
        Task<OperationResult> PurchasedItem(PurchasedItemModel Command);
        Task<List<PurchasedItemModel>> GetPurchasedList(long UserId);
        Task<List<SuppliedItemModel>> GetSuppliedList(long UserId);
    }
}