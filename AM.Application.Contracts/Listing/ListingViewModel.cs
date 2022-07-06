using System;
using System.Collections.Generic;
using AM.Application.Contracts.Deal;
using AM.Application.Contracts.User;

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
        public string? Type { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? DeliveryMethod { get; set; }
        public CreateDeliveryLocation? DeliveryLocation { get; set; }
        public string? Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string? Currency { get; set; }
        public bool PublicStatus { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsService { get; set; }

        public List<DealViewModel>? DealList { get; set; }
        public long PurchaseCount { get; set; }
        public long SupplyCount { get; set; }
        public long? NaceId { get; set; }
    }


    public class PurchasedItemViewModel
    {
        public long UserId { get; set; }
    }

    public class SuppliedItemViewModel
    {
        public long UserId { get; set; }
    }
}