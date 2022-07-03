using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;

namespace AM.Domain.NegotiateAggregate
{
    public class Negotiate : BaseEntity<long>
    {
        protected Negotiate()
        {
        }

        public Negotiate(long listingId, long buyerId, long sellerId)
        {
            ListingId = listingId;
            BuyerId = buyerId;
            SellerId = sellerId;
            IsFinished = false;
            IsCanceled = false;
            IsActive = false;
            IsRejected = false;
            QuotationConfirm = false;
            QuotationSent = false;
            CreationTime = DateTime.Now;
            Messages = new List<Message>();
        }

        public void AttachDealId(long dealId)
        {
            DealId = dealId;
        }

        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public List<UserNegotiate>? UserNegotiate { get; private set; }
        public long? DealId { get; private set; }
        public Deal? Deal { get; private set; }
        public long BuyerId { get; private set; }
        public long SellerId { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsCanceled { get; private set; }
        public bool IsRejected { get; private set; }
        public bool QuotationConfirm { get; private set; }
        public bool QuotationSent { get; private set; }
        public List<Message> Messages { get; private set; }

        public void AddMessage(string? messageBody, long userId, bool userEntity, string? filePathString)
        {
            Messages.Add(new Message(messageBody, userId, userEntity, filePathString));
        }

        public void Finished()
        {
            IsFinished = true;
        }
        public void Canceled()
        {
            IsCanceled = true;
        }
        public void Activate()
        {
            IsActive = true;
        }
        public void RejectDeal()
        {
            IsRejected = true;
        }
        public void QuotationHasSent()
        {
            QuotationSent = true;
        }
        public void QuotationConfirmed()
        {
            QuotationConfirm = true;
        }
    }
}