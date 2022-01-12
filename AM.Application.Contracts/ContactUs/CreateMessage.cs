using System;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.ContactUs
{
    public class CreateMessage
    {
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? FullName { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Subject { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Body { get; set; }
    }

    public class ContactUsViewModel : CreateMessage
    {
        public long Id { get; set; }
        public DateTime CreationTime { get; set; }
        public bool IsReed { get; set; }
    }
}