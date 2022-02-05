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

        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public List<User>? Users { get; private set; }
        }
}