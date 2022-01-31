﻿using System.Collections.Generic;
using System.Linq;
using _0_Framework.Domain;
using AM.Domain.ListingAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain.NegotiateAggregate
{
    public class Negotiate : BaseEntity<long>
    {
        protected Negotiate()
        {
        }

        public Negotiate(long listingId, long buyyerId, long sellerId)
        {
            ListingId = listingId;
            BuyyerId = buyyerId;
            SellerId = sellerId;
            IsFinished = false;
            Messages = new List<Message>();
        }

        public long ListingId { get; private set; }
        public Listing? Listing { get; private set; }
        public long BuyyerId { get; private set; }
        public long SellerId { get; private set; }
        public bool IsFinished { get; private set; }
        public List<Message>? Messages { get; private set; }

        public void AddMessage(string messageBody, long userId, bool userEntity)
        {
            Messages.Add(new Message(messageBody, userId, userEntity));
        }

        public void Finished()
        {
            IsFinished = true;
        }

    }
}