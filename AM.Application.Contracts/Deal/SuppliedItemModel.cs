namespace AM.Application.Contracts.Deal
{
    public class SuppliedItemModel
    {
        public long ListingId { get; set; }
        public long UserId { get; set; }
        public double Amount { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPaid { get; set; }
        public string? Currency { get; set; }
    }
}