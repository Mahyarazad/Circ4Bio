using System;
using System.Collections.Generic;

namespace AM.Application.Contracts.Listing
{
    public class ListingViewModel
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public DateTime CreationTime { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Name { get; set; }
        public long? Type { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public bool Status { get; set; }
        public bool IsDeleted { get; set; }
        public List<DealViewModel>? DealList { get; set; }
        public List<PurchasedItemViewModel>? PurchaseList { get; set; }
        public List<SuppliedItemViewModel>? SupplyList { get; set; }
    }

    public class DealViewModel
    {
    }

    public class PurchasedItemViewModel
    {
    }

    public class SuppliedItemViewModel
    {
    }
}