using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using _0_Framework;
using Microsoft.AspNetCore.Http;

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
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = ValidationMessages.PasswordNotMatch)]
        [Required(ErrorMessage = ValidationMessages.ConfirmPassword)]

        public string? ConfirmPassword { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? PictureString { get; set; }
        [Required(ErrorMessage = ValidationMessages.SelectUserType)]

        public int RoleId { get; set; }

        public List<Usertype>? TypeList { get; set; }



    }

    public class RememberMe
    {
        public string? Email { get; set; }
    }

}