﻿using System.ComponentModel.DataAnnotations;
using _0_Framework;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AM.Application.Contracts.Listing
{
    public class CreateListing
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = ValidationMessages.SizeError1M)]
        public IFormFile? Image { get; set; }
        public string? ImageString { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? DeliveryMethod { get; set; }
        // It can be kg, lit, ton, peice and etc.
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Unit { get; set; }
        [Range(1, 9999999999)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public double UnitPrice { get; set; }
        [Range(1, 9999999999)]
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public double Amount { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Currency { get; set; }
        // 0 for public and 1 for private
        public bool Status { get; set; }
        public bool IsService { get; set; }
    }
    public class EditListing : CreateListing
    {
        public long Id { get; set; }
    }
}