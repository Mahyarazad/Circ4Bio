using System;
using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain
{
    public class Deal : BaseEntity<long>
    {
        protected Deal()
        {
        }

        public List<User>? Users { get; private set; }
        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public double Cost { get; private set; }
        public double Amount { get; private set; }
        public string? Location { get; private set; }
        public string? TrackingCode { get; private set; }
        public string? ContractFile { get; private set; }
        public int ClosingStatus { get; private set; }
        public bool PaymentStatus { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime DueTime { get; private set; }
    }
}