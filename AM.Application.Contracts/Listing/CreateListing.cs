using System;
using System.ComponentModel.DataAnnotations;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AM.Application.Contracts.Listing
{
    public class CreateListing
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(50, ErrorMessage = ValidationMessages.MaxCharName)]
        public string? Name { get; set; }
        public string? Type { get; set; }
        [MaxLength(2000, ErrorMessage = ValidationMessages.MaxChar2000)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Description { get; set; }
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = ValidationMessages.SizeError1M)]
        public IFormFile? Image { get; set; }
        public string? ImageString { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? DeliveryMethod { get; set; }
        // It can be kg, lit, ton, peice and etc.
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = ValidationMessages.UnitError)]
        public string? Unit { get; set; }
        [Range(0, 9999999999)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get; set; }
        [Range(1, 9999999999)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public double Amount { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        [MaxLength(3, ErrorMessage = ValidationMessages.Currency)]
        public string? Currency { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryLocation)]
        public int Location { get; set; }
        public CreateDeliveryLocation? DeliveryLocation { get; set; }
        // 0 for public and 1 for private
        public bool Status { get; set; }
        public bool IsService { get; set; }
        public long NaceId { get; set; }

    }
    public class EditListing : CreateListing
    {
        public long Id { get; set; }
        public long OwnerUserId { get; set; }
        public NaceDataDTO NaceData { get; set; }
    }
}