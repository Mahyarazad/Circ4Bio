using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Domain;
using AM.Domain.BlogAggregate;
using AM.Domain.ListingAggregate;
using AM.Domain.NegotiateAggregate;
using AM.Domain.NotificationAggregate;
using AM.Domain.RoleAggregate;
using AM.Domain.Supplied.PurchasedAggregate;

namespace AM.Domain.UserAggregate
{
    public class User : BaseEntity<long>
    {
        protected User()
        {

        }
        public User(string? email, string password, Guid activationGuid, int roleId, bool status)
        {

            Email = email;
            UserName = email?.Substring(0, email.IndexOf('@'));
            Password = password;
            IsActive = false;
            Status = status;
            ActivationGuid = activationGuid;
            RoleId = roleId;
            Avatar = "default-avatar.png";
            Notifications = new List<Notification>();
            DeliveryLocations = new List<DeliveryLocation>();
        }

        public void Edit(string? firstName, string? lastName, string userId
            , string address, string city, string country, long postalCode
            , double latitude, double longitude, string description, string companyName, long vatNumber
            , string avatar, string webUrl, string linkdinUrl
            , string twitterUrl, string instagramUrl, string faceBookUrl, int roleID)
        {
            FirstName = firstName;
            LastName = lastName;
            UserName = userId;
            Address = address;
            City = city;
            Country = country;
            PostalCode = postalCode;
            Latitude = latitude;
            Longitude = longitude;
            Description = description;
            CompanyName = companyName;
            VatNumber = vatNumber;

            if (!string.IsNullOrWhiteSpace(avatar))
                Avatar = avatar;
            WebUrl = webUrl;
            LinkdinUrl = linkdinUrl;
            TwitterUrl = twitterUrl;
            InstagramUrl = instagramUrl;
            FaceBookUrl = faceBookUrl;
            RoleId = roleID;
        }

        public void AddDeliveryLocation(long userId, string? location)
        {
            DeliveryLocations.Add(new DeliveryLocation(userId, location));
        }

        public bool RemoveDeliveryLocation(int Id)
        {
            var itemToRemove = DeliveryLocations.FirstOrDefault(x => x.Id == Id);
            if (itemToRemove != null)
                return DeliveryLocations.Remove(itemToRemove);
            return false;
        }
        public void ChangePassword(string password)
        {
            Password = password;
        }


        public void ActivateUser()
        {
            IsActive = true;
        }

        public void DeactivateUser()
        {
            IsActive = false;
        }

        public void ActivateUserStatus()
        {
            Status = true;
        }

        public void DeactivateUserStatus()
        {
            Status = false;
        }

        public string? FirstName { get; private set; }
        public string? LastName { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? Password { get; private set; }
        public string? UserName { get; private set; }
        public string? Address { get; private set; }
        public List<DeliveryLocation> DeliveryLocations { get; private set; }
        public string? City { get; private set; }
        public string? Country { get; private set; }
        public long PostalCode { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string? Description { get; private set; }
        public string? CompanyName { get; private set; }
        public long VatNumber { get; private set; }
        public bool Status { get; private set; }
        public int Type { get; private set; }
        public int User_Type { get; private set; }
        public string? Avatar { get; private set; }
        public string? WebUrl { get; private set; }
        public string? LinkdinUrl { get; private set; }
        public string? TwitterUrl { get; private set; }
        public string? InstagramUrl { get; private set; }
        public string? FaceBookUrl { get; private set; }
        public bool IsActive { get; private set; }
        public Guid ActivationGuid { get; private set; }
        public int RoleId { get; private set; }
        public Role? Role { get; private set; }
        public List<UserNegotiate>? UserNegotiate { get; private set; }
        public List<Listing>? Listings { get; private set; }
        public long? SuppliedItemId { get; private set; }
        public SuppliedItem? SuppliedItem { get; private set; }
        public long? PurchasedItemId { get; private set; }
        public PurchasedItem? PurchasedItem { get; private set; }
        public List<Notification>? Notifications { get; private set; }
        public List<Blog>? Blogs { get; private set; }

    }
}