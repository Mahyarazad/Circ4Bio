using System;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.ResetPassword
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        public string? Email { get; set; }
        [Required(ErrorMessage = ValidationMessages.Password)]
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = ValidationMessages.PasswordNotMatch)]
        [Required(ErrorMessage = ValidationMessages.ConfirmPassword)]
        public string? ConfirmPassword { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpirationTime { get; set; }
        public long UserId { get; set; }
        public bool IsValid { get; set; }
    }
}