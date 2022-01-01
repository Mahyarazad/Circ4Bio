using Microsoft.AspNetCore.Http;

namespace AM.Application.Contracts.User
{

    public class RegisterUser
    {
        public string FullName { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public IFormFile ProfilePicture { get; set; }
        public string PictureString { get; set; }
        public int RoleId { get; set; }
    }
}
