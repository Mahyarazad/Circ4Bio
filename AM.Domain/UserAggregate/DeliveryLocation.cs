using System.Collections.Generic;

namespace AM.Domain.UserAggregate
{
    public class DeliveryLocation
    {
        protected DeliveryLocation() { }
        public DeliveryLocation(long userId, string? name, string? addressLineOne, string? addressLineTwo,
            string? city, string? country, double latitude, double longitude, long postalCode)
        {
            UserId = userId;
            Name = name;
            AddressLineOne = addressLineOne;
            AddressLineTwo = addressLineTwo;
            City = city;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
            PostalCode = postalCode;
        }

        public int Id { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        public string? Name { get; private set; }
        public string? AddressLineOne { get; private set; }
        public string? AddressLineTwo { get; private set; }
        public string? City { get; private set; }
        public string? Country { get; private set; }
        public long PostalCode { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }

        public void _EditDeliveryLocation(long userId, string? name, string? addressLineOne, string? addressLineTwo,
            string? city, string? country, double latitude, double longitude, long postalCode)
        {
            UserId = userId;
            Name = name;
            AddressLineOne = addressLineOne;
            AddressLineTwo = addressLineTwo;
            City = city;
            Country = country;
            Latitude = latitude;
            Longitude = longitude;
            PostalCode = postalCode;
        }
    }
}