using System.Collections.Generic;

namespace AM.Application.Contracts.Negotiate
{
    public class CreateNegotiate
    {
        public long BuyyerId { get; set; }
        public long SellerId { get; set; }
        public long ListingId { get; set; }
        public long NegotiateId { get; set; }
    }

    public class NegotiateViewModel : CreateNegotiate
    {
        public string? ListingName { get; set; }
        public string? ImageString { get; set; }
        public string? BuyyerImageString { get; set; }
        public string? SellerName { get; set; }
        public string? BuyyerName { get; set; }
        public string? SellerEmail { get; set; }
        public string? BuyyerEmail { get; set; }
        public string? ItemType { get; set; }
        public string? SellerImageString { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string? Currency { get; set; }

    }
}