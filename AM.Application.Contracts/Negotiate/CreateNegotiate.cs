using System;
using System.Collections.Generic;
using AM.Application.Contracts.User;

namespace AM.Application.Contracts.Negotiate
{
    public class CreateNegotiate
    {
        public long BuyerId { get; set; }
        public long SellerId { get; set; }
        public long ListingId { get; set; }
        public long NegotiateId { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsFinished { get; set; }
        public bool IsActive { get; set; }
        public bool QuatationSent { get; set; }
        public bool QuatationConfirm { get; set; }
        public bool IsRejected { get; set; }
    }

    public class NegotiateViewModel : CreateNegotiate
    {
        public string? ListingName { get; set; }
        public string? ImageString { get; set; }
        public string? BuyerImageString { get; set; }
        public string? SellerName { get; set; }
        public DateTime CreationTime { get; set; }
        public string? BuyerName { get; set; }
        public string? SellerEmail { get; set; }
        public string? BuyerEmail { get; set; }
        public string? ItemType { get; set; }
        public string? SellerImageString { get; set; }
        public string? DeliveryMethod { get; set; }
        public string? Unit { get; set; }
        public double UnitPrice { get; set; }
        public double Amount { get; set; }
        public string? Currency { get; set; }
        public CreateDeliveryLocation DeliveryLocation { get; set; }

    }

    public class UserInfoMessaging
    {
        public string? ImageString { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Letter { get; set; }
    }
}