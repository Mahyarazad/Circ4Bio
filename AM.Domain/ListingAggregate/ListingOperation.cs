using System;

namespace AM.Domain.ListingAggregate
{
    public class ListingOperation
    {
        protected ListingOperation()
        {
        }

        public ListingOperation(bool operationType, long listingId, double currentAmount, double count
            , string? description, long dealId, long userId)
        {
            OperationType = operationType;
            ListingId = listingId;
            CurrentAmount = currentAmount;
            Count = count;
            Description = description;
            DealId = dealId;
            UserId = userId;
            OperationTime = DateTime.Now;
        }

        public int Id { get; private set; }
        public bool OperationType { get; private set; }
        public long ListingId { get; private set; }
        public double CurrentAmount { get; private set; }
        public DateTime OperationTime { get; private set; }
        public double Count { get; private set; }
        public string? Description { get; private set; }
        public long DealId { get; private set; }
        public long UserId { get; private set; }
        public Listing? Listing { get; private set; }

    }
}