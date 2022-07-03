using System;
using AM.Application.Contracts.User;

namespace AM.Application.Contracts.Deal
{
    public class DealViewModel : EditDeal
    {
        public bool IsCanceled { get; set; }
        public bool IsActive { get; set; }
        public bool IsFinished { get; set; }
        public bool IsRejected { get; set; }
        public bool QuotationSent { get; set; }
        public string? ItemName { get; set; }
        public DateTime CreationTime { get; set; }
        public string? ContractFileString { get; set; }
        public CreateDeliveryLocation? DeliveryLocation { get; set; }
        public string? PaymentId { get; set; }
        public DateTime PaymentTime { get; set; }
        public string? PayerEmail { get; set; }
        public string? PayerFirstName { get; set; }
        public string? PayerLastName { get; set; }
        public double PaidAmount { get; set; }
        public double TransactionFee { get; set; }

    }
}