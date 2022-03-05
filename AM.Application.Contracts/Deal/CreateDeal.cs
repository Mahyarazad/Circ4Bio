using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Http;

namespace AM.Application.Contracts.Deal
{
    public class CreateDeal
    {
        public UserViewModel? Seller { get; set; }
        public UserViewModel? Buyer { get; set; }
        public long ListingId { get; set; }
        public ListingViewModel? Listing { get; set; }
        public long NegotiateId { get; set; }
        [Range(0, 999999999999999)]
        public double DeliveryCost { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryMethod)]
        public string? DeliveryMethod { get; set; }
        public double TotalCost { get; set; }
        [Range(1, 999999999999999)]
        public double Amount { get; set; }
        [Required(ErrorMessage = ValidationMessages.Currency)]
        public string? Currency { get; set; }
        [Required(ErrorMessage = ValidationMessages.DeliveryLocation)]
        public string? Location { get; set; }
        public string? TrackingCode { get; set; }
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = ValidationMessages.SizeError1M)]
        public IFormFile? ContractFile { get; set; }
        public DateTime DueTime { get; set; }

    }

    public class EditDeal : CreateDeal
    {
        public long DealId { get; set; }
        public string? Unit { get; set; }
    }

    public class DealViewModel : EditDeal
    {
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsFinished { get; set; }
        public bool IsRejected { get; set; }
        public bool QuatationSent { get; set; }
        public string? ItemName { get; set; }
        public DateTime CreationTime { get; set; }
        public string? ContractFileString { get; set; }
        public CreateDeliveryLocation? DeliveryLocation { get; set; }
    }
}