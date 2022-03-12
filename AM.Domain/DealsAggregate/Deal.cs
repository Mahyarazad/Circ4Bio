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
            , string? curency, double amount, int deliveryLocationId, string? trackingCode
            , string? contractFile, DateTime dueDate, long listingId
            , long negotiateId, long buyerId, long sellerId)
        {
            DeliveryCost = deliveryCost;
            DeliveryMethod = deliveryMethod;
            TotalCost = totalCost;
            Unit = unit;
            Currency = curency;
            Amount = amount;
            DeliveryLocationId = deliveryLocationId;
            TrackingCode = trackingCode;
            ContractFile = contractFile;
            PaymentStatus = false;
            IsDeleted = false;
            IsActive = false;
            IsRejected = false;
            IsFinished = false;
            QuatationSent = false;
            DueTime = dueDate;
            ListingId = listingId;
            NegotiateId = negotiateId;
            StartTime = DateTime.Now;
            BuyerId = buyerId;
            SellerId = sellerId;
            PaymentInfo = null;
        }

        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public long NegotiateId { get; private set; }
        public int DeliveryLocationId { get; private set; }
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
        public PaymentInfo? PaymentInfo { get; private set; }
        public string? PaymentDate { get; private set; }

        public string? ContractFile { get; private set; }
        //Deal Status(IsFinished[true] = Closed  , IsFinished[false] = Open)
        //Deal Status(IsRejected[true] = Rejected , IsRejected[false] = Pending)
        //Deal Status(IsDeleted[true] = Canceled by Buyer , IsDeleted[false] = Pending)
        //Deal Status(IsActivate[true] = When the Contract Approved by both parties , IsDeleted[false] = Pending)
        public bool PaymentStatus { get; private set; }
        public bool IsDeleted { get; private set; }
        public bool IsRejected { get; private set; }
        public bool IsActive { get; private set; }
        public bool QuatationSent { get; private set; }
        public bool IsFinished { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime DueTime { get; private set; }

        public void RejectDeal()
        {
            IsRejected = true;
        }
        public void QuatationHasSent()
        {
            QuatationSent = true;
        }
        public void CancelDeal()
        {
            IsDeleted = true;
        }

        public void ActivateDeal(string trackingCode)
        {
            IsActive = true;
            TrackingCode = trackingCode;
        }

        public void PaymentFinished(PaymentInfo paymentInfo)
        {
            IsFinished = true;
            PaymentInfo = new PaymentInfo(paymentInfo.PaymentId, paymentInfo.PaymentTime, paymentInfo.PayerEmail,
                paymentInfo.PayerFirstName, paymentInfo.PayerLastName);

        }

        public void PaymentReceived()
        {
            PaymentStatus = true;
        }

        public void Edit(double deliveryCost, string? deliveryMethod, double totalCost, string? unit
            , string? curency, double amount, int deliveryLocationId, string? contractFile)
        {
            DeliveryCost = deliveryCost;
            DeliveryMethod = deliveryMethod;
            TotalCost = totalCost;
            Unit = unit;
            Currency = curency;
            Amount = amount;
            DeliveryLocationId = deliveryLocationId;
            ContractFile = contractFile;
            PaymentStatus = false;
            IsDeleted = false;
            IsActive = false;
            IsFinished = false;
        }
    }
}