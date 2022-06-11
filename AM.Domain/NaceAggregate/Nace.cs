using System.Collections.Generic;
using System.Runtime.InteropServices;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;

namespace AM.Domain.NaceAggregate
{
    public class Nace : BaseEntity<long>
    {
        protected Nace() { }
        public string? Title { get; private set; }
        public List<Detail>? Items { get; private set; }
        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
    }
}