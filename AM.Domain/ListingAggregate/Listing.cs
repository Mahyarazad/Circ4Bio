using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using _0_Framework.Domain;
using AM.Domain.NaceAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain.ListingAggregate
{
    public class Listing : BaseEntity<long>
    {
        protected Listing()
        {
        }

        public Listing(string name, string type,
            string description, string image, string deliveryMethod,
            string unit, double unitPrice, double amount, bool status,
            long userId, bool isService, string currency, int deliveryLocationId)
        {
            Name = name;
            Type = type;
            Description = description;
            DeliveryMethod = deliveryMethod;
            DeliveryLocationId = deliveryLocationId;
            Image = image;
            IsService = isService;
            if (!isService)
            {

                Amount = amount;
                ListingOperations = new List<ListingOperation>
                {
                    new ListingOperation(true, this.Id, amount, amount, "Intilized Amount", 0 , userId)
                };
            }
            else
            {
                Amount = 0;
                ListingOperations = new List<ListingOperation>
                {
                    new ListingOperation(true, this.Id, 0, 0, "Intilized Service", 0 , userId)
                };
            }
            Unit = unit;
            UnitPrice = unitPrice;
            Status = status;
            IsDeleted = false;
            HasAmount = true;
            UserId = userId;
            Currency = currency;
            CreationTime = DateTime.Now;

        }

        public void Edit(string name,
            string description, string image, string deliveryMethod,
            string unit, double unitPrice, string currency, int deliveryLocationId)
        {
            Name = name;
            Description = description;
            if (!string.IsNullOrWhiteSpace(image))
                Image = image;
            DeliveryMethod = deliveryMethod;
            DeliveryLocationId = deliveryLocationId;
            Unit = unit;
            UnitPrice = unitPrice;
            Currency = currency;
        }

        public void MarkDeleted()
        {
            IsDeleted = true;
        }

        public void MarkPublic()
        {
            Status = false;
        }

        public void MarkPrivate()
        {
            Status = true;
        }

        public string? Name { get; private set; }
        public string? Type { get; private set; }
        public string? Description { get; private set; }
        public string? Image { get; private set; }
        public string? DeliveryMethod { get; private set; }
        public int DeliveryLocationId { get; private set; }
        // It can be kg, lit, ton, piece and etc.
        public string? Unit { get; private set; }
        public double UnitPrice { get; private set; }
        public string Currency { get; private set; }
        public double Amount { get; private set; }
        public bool HasAmount { get; private set; }
        public bool IsService { get; private set; }
        // 0 for public and 1 for private
        public bool Status { get; private set; }
        public bool IsDeleted { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        //public Nace? Nace { get; private set; }
        public NaceData? NaceData { get; private set; }
        public List<Deal>? DealList { get; private set; }
        public List<Negotiate>? NegotiateList { get; private set; }
        public List<PurchasedItem>? PurchaseList { get; private set; }
        public List<SuppliedItem>? SupplyList { get; private set; }
        public List<ListingOperation>? ListingOperations { get; private set; }

        public double CalculateCurrentAmount()
        {
            var incoming = ListingOperations?.Where(x => x.OperationType).Sum(x => x.Count);
            var outgoing = ListingOperations?.Where(x => !x.OperationType).Sum(x => x.Count);
            return (double)(incoming - outgoing);
        }

        public void Increment(string description, double count, long dealId, long userId)
        {
            var currentAmount = CalculateCurrentAmount() + count;
            var listingOperation = new ListingOperation(true, Id, currentAmount, count, description, dealId, userId);
            ListingOperations?.Add(listingOperation);
            Amount = currentAmount;
            HasAmount = CalculateCurrentAmount() > 0;
        }

        public void Decrement(string description, double count, long dealId, long userId)
        {
            var currentAmount = CalculateCurrentAmount() - count;
            var inventoryOperation = new ListingOperation(false, Id, currentAmount, count
                , description, dealId, userId);
            ListingOperations?.Add(inventoryOperation);
            Amount = currentAmount;
            HasAmount = CalculateCurrentAmount() > 0;
        }
    }
}