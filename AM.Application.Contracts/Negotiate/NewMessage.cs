using System;
using System.ComponentModel.DataAnnotations;
using _0_Framework;
using _0_Framework.Application;
using Microsoft.AspNetCore.Http;

namespace AM.Application.Contracts.Negotiate
{
    public class MessageViewModel : NewMessage
    {
        public long MessageId { get; set; }
        public long BuyerId { get; set; }
        public long SellerId { get; set; }
        public string? FileString { get; set; }
        public string? BuyyerLetter { get; set; }
        public string? SellerLetter { get; set; }
        public string? BuyyerImageString { get; set; }
        public string? SellerImageString { get; set; }
        public DateTime CreationTime { get; set; }
    }

    public class NewMessage
    {
        public long NegotiateId { get; set; }
        public long UserId { get; set; }

        [Required(ErrorMessage = ApplicationMessage.EmptyMessage)]
        public string MessageBody { get; set; }

        [MaxFileSize(1 * 1024 * 1024, ErrorMessage = ValidationMessages.SizeError1M)]
        // [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$", ErrorMessage = ValidationMessages.InvalidUploadFileFormat)]
        [FileExtensionLimit(new string[] { ".pdf" }, ErrorMessage = ValidationMessages.InvalidUploadFileFormat)]
        public IFormFile? File { get; set; }
        public bool UserEntity { get; set; }
        public bool Receiver
        {
            get
            {
                return !UserEntity;
            }

        }
    }
}