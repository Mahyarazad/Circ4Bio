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
        [MinLength(8, ErrorMessage = ValidationMessages.StrongerPassword8Character)]
        [MaxLength(32, ErrorMessage = ValidationMessages.StrongerPassword8Character)]
        //@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z0-9])).*"
        [RegularExpression(@"^((?=.*\d)(?=.*[a-z])(?=.*[A-Z])).*"
            , ErrorMessage = ValidationMessages.StrongerPassword)]
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