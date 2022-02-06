using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain
{
    public class Deal : BaseEntity<long>
    {
        protected Deal()
        {
        }

        public Deal(double deliveryCost, string? deliveryMethod, double totalCost, string? unit
            , string? curency, double amount, string? location, string? trackingCode
            , string? contractFile, DateTime dueDate, long listingId
            , long negotiateId, long buyerId, long sellerId)
        {
            DeliveryCost = deliveryCost;
            DeliveryMethod = deliveryMethod;
            TotalCost = totalCost;
            Unit = unit;
            Currency = curency;
            Amount = amount;
            Location = location;
            TrackingCode = trackingCode;
            ContractFile = contractFile;
            PaymentStatus = false;
            IsDeleted = false;
            IsActive = false;
            IsFinished = false;
            DueTime = dueDate;
            ListingId = listingId;
            NegotiateId = negotiateId;
            StartTime = DateTime.Now;
            BuyerId = buyerId;
            SellerId = sellerId;
        }

        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public long NegotiateId { get; private set; }
        public long BuyerId { get; private set; }
        public long SellerId { get; private set; }
        public List<Negotiate>? Negotiates { get; private set; }
        public double DeliveryCost { get; private set; }
        public double TotalCost { get; private set; }
        public double Amount { get; private set; }
        public string? Unit { get; private set; }
        public string? DeliveryMethod { get; private set; }
        public string? Currency { get; private set; }
        public string? Location { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string? TrackingCode { get; private set; }
        public string? ContractFile { get; private set; }
        //Deal Status(IsFinished[true] = Closed  , IsFinished[false] = Open)
        //Deal Status(IsDeleted[true] = Rejected , IsDeleted[false] = Pending)
        //Deal Status(IsActivate[true] = When the Contract Approved by both parties , IsDeleted[false] = Pending)
        public bool PaymentStatus { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsFinished { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime DueTime { get; private set; }

        public void RejectDeal()
        {
            IsDeleted = true;
        }

        public void FinishDeal()
        {
            IsFinished = true;
        }
        public void ActivateDeal()
        {
            IsActive = true;
        }
        public void PaymentReceived()
        {
            PaymentStatus = true;
        }
    }
}