using System;

namespace AM.Application.Contracts.Listing
{
    public class ListingOperationLog
    {
        public int Id { get; set; }
        public bool OperationType { get; set; }
        public long ListingId { get; set; }
        public double CurrentAmount { get; set; }
        public DateTime OperationTime { get; set; }
        public double Count { get; set; }
        public string? Description { get; set; }
        public string? Unit { get; set; }
        public long DealId { get; set; }
        public long UserId { get; set; }
    }
}