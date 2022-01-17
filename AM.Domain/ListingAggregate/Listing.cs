using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain.ListingAggregate
{
    public class Listing : BaseEntity<long>
    {
        protected Listing()
        {
        }

        public string? Name { get; private set; }
        public string? Type { get; private set; }
        public string? Description { get; private set; }
        public string? Image { get; private set; }
        public string? DeliveryMethod { get; private set; }
        // It can be kg, lit, ton, peice and etc.
        public string? Unit { get; private set; }
        public double UnitPrice { get; private set; }
        public double Amount { get; private set; }
        // 0 for public and 1 for private
        public bool Status { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        public List<Deal>? DealList { get; private set; }
        public List<PurchasedItem>? PurchaseList { get; private set; }
        public List<SuppliedItem>? SupplyList { get; private set; }
    }
}