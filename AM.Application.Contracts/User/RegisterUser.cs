using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using _0_Framework;

namespace AM.Application.Contracts.User
{

    public class RegisterUser
    {
        public string? FullName { get; set; }
        [Required(ErrorMessage = ValidationMessages.EmailRequired)]
        public string? Email { get; set; }
        public int Type { get; set; }
        public string? PhoneNumber { get; set; }
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
        public string? PictureString { get; set; }
        [Required(ErrorMessage = ValidationMessages.SelectUserType)]
        public int RoleId { get; set; }
        public bool Status { get; set; }
        public List<Usertype>? TypeList { get; set; }
        public bool RememberMe { get; set; }
    }

    public class RememberMe
    {
        public string? Email { get; set; }
    }

}