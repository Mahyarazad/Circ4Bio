using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.User
{
    public class CreateDeliveryLocation
    {
        public long UserId { get; set; }

        [MaxLength(150, ErrorMessage = ValidationMessages.MaxCharName)]
        [Required(ErrorMessage = ValidationMessages.DeliveryLocationName)]
        public string? Name { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryLocation)]
        [MaxLength(150, ErrorMessage = ValidationMessages.MaxCharName)]
        public string? AddressLineOne { get; set; }
        public string? AddressLineTwo { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryLocationCity)]
        public string? City { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryLocationCountry)]
        public string? Country { get; set; }
        public long PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int LocationId { get; set; }
    }
}