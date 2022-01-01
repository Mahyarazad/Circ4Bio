using System;
using _0_Framework.Domain;
using AM.Domain.RoleAggregate;

namespace AM.Domain.UserAggregate
{
    public class User : BaseEntity<long>
    {
        protected User()
        {

        }
        public User(string firstName, string lastName, string email, string password, string address,
            string city, string country, long postalCode, double latitude, double longitude,
            string description, string companyName, long vatNumber, int status, int type, int userType,
            string avatar, string webUrl, string linkdinUrl, string twitterUrl, string instagramUrl,
            string faceBookUrl, int roleId)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Address = address;
            City = city;
            Country = country;
            PostalCode = postalCode;
            Latitude = latitude;
            Longitude = longitude;
            Description = description;
            CompanyName = companyName;
            VatNumber = vatNumber;
            Status = status;
            Type = type;
            User_Type = userType;
            Avatar = avatar;
            WebUrl = webUrl;
            LinkdinUrl = linkdinUrl;
            TwitterUrl = twitterUrl;
            InstagramUrl = instagramUrl;
            FaceBookUrl = faceBookUrl;
            RoleId = roleId;
        }

        public void Edit(string firstName, string lastName, string email, string password
            , string address, string city, string country, long postalCode
            , double latitude, double longitude, string description, string companyName, long vatNumber
            , int status, int type, int userType, string avatar, string webUrl, string linkdinUrl
            , string twitterUrl, string instagramUrl, string faceBookUrl, int roleID)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            Address = address;
            City = city;
            Country = country;
            PostalCode = postalCode;
            Latitude = latitude;
            Longitude = longitude;
            Description = description;
            CompanyName = companyName;
            VatNumber = vatNumber;
            Status = status;
            Type = type;
            User_Type = userType;
            if (!string.IsNullOrWhiteSpace(avatar))
                Avatar = avatar;
            WebUrl = webUrl;
            LinkdinUrl = linkdinUrl;
            TwitterUrl = twitterUrl;
            InstagramUrl = instagramUrl;
            FaceBookUrl = faceBookUrl;
            RoleId = roleID;
        }

        public void ChangePassword(string password)
        {
            Password = password;
        }

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Password { get; private set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string Country { get; private set; }
        public long PostalCode { get; private set; }
        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
        public string Description { get; private set; }
        public string CompanyName { get; private set; }
        public long VatNumber { get; private set; }
        public int Status { get; private set; }
        public int Type { get; private set; }
        public int User_Type { get; private set; }
        public string Avatar { get; private set; }
        public string WebUrl { get; private set; }
        public string LinkdinUrl { get; private set; }
        public string TwitterUrl { get; private set; }
        public string InstagramUrl { get; private set; }
        public string FaceBookUrl { get; private set; }
        public int RoleId { get; private set; }
        public Role Role { get; private set; }
    }
}