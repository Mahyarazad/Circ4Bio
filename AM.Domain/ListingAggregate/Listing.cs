using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Domain.Supplied.PurchasedAggregate;
using AM.Domain.UserAggregate;

namespace AM.Domain.ListingAggregate
{
    public class Listing : BaseEntity<long>
    {
        protected Listing()
        {
        }

        public Listing(string? name, long type,
            string? description, string? image, string? deliveryMethod,
            string? unit, double unitPrice, double amount, bool status,
            long userId, List<Deal>? dealList, List<PurchasedItem>? purchasedItems
            , List<SuppliedItem>? suppliedItems)
        {
            Name = name;
            Type = type;
            Description = description;
            Image = image;
            DeliveryMethod = deliveryMethod;
            Unit = unit;
            UnitPrice = unitPrice;
            Amount = amount;
            Status = status;
            IsDeleted = false;
            UserId = userId;
            DealList = dealList;
            PurchaseList = purchasedItems;
            SupplyList = suppliedItems;

        }
        public void Edit(string? name,
            string? description, string? image, string? deliveryMethod,
            string? unit, double unitPrice, double amount)
        {
            Name = name;
            Description = description;
            Image = image;
            DeliveryMethod = deliveryMethod;
            Unit = unit;
            UnitPrice = unitPrice;
            Amount = amount;
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
        public long Type { get; private set; }
        public string? Description { get; private set; }
        public string? Image { get; private set; }
        public string? DeliveryMethod { get; private set; }
        // It can be kg, lit, ton, peice and etc.
        public string? Unit { get; private set; }
        public double UnitPrice { get; private set; }
        public double Amount { get; private set; }
        // 0 for public and 1 for private
        public bool Status { get; private set; }
        public bool IsDeleted { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        public List<Deal>? DealList { get; private set; }
        public List<PurchasedItem>? PurchaseList { get; private set; }
        public List<SuppliedItem>? SupplyList { get; private set; }
    }
}