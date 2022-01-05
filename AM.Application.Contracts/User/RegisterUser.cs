using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using _0_Framework;
using Microsoft.AspNetCore.Http;

namespace AM.Application.Contracts.User
{
    public class RegisterUser
    {
        public string? FullName { get; set; }
        public string? UserId { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Email { get; set; }
        public int Type { get; set; }
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? Password { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public string? ConfirmPassword { get; set; }
        public IFormFile? ProfilePicture { get; set; }
        public string? PictureString { get; set; }
        [Required(ErrorMessage = ValidationMessages.IsRequired)]
        public int RoleId { get; set; }

        public List<Usertype>? TypeList { get; set; }



    }

    public class Usertype
    {
        public Usertype(int typeId, string typeName)
        {
            TypeId = typeId;
            TypeName = typeName;
        }

        public int TypeId { get; set; }
        public string TypeName { get; set; }

        public List<Usertype> GetUserTypeList()
        {
            return new List<Usertype> {
                new Usertype(3, "Technology Provider"),
                new Usertype(4, "Plant"),
                new Usertype(5, "Supplier of Raw Material"),
                new Usertype(6, "Customer of Raw Material")
            };
        }
    }

}