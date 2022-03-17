using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain.Supplied.PurchasedAggregate
{
    public class PurchasedItem : BaseEntity<long>
    {
        protected PurchasedItem()
        {
        }

        public PurchasedItem(long listingId, long userId, double amount, double unitPrice
            , double totalPaid, string currency)
        {
            ListingId = listingId;
            UserId = userId;
            Amount = amount;
            UnitPrice = unitPrice;
            TotalPaid = totalPaid;
            Currency = currency;
        }

        public long ListingId { get; private set; }
        public long UserId { get; private set; }
        public double Amount { get; private set; }
        public double UnitPrice { get; private set; }
        public double TotalPaid { get; private set; }
        public string Currency { get; private set; }
        public Listing? Listing { get; private set; }
        public User? Users { get; private set; }
    }
}