using System.Collections.Generic;

namespace _0_Framework.Application
{
    public class AuthViewModel
    {
        public AuthViewModel()
        {

        }

        public AuthViewModel(long id, string email, string fullname
            , string roleId, bool rememberMe, string profilePicture, List<int> permissions, string password, bool isActive, bool status)
        {
            Id = id;
            Email = email;
            Fullname = fullname;
            RoleId = roleId;
            RememberMe = rememberMe;
            ProfilePicture = profilePicture;
            Permissions = permissions;
            Password = password;
            Status = status;
            IsActive = isActive;
        }
        public long Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Fullname { get; set; }
        public string? RoleId { get; set; }
        public bool RememberMe { get; set; }
        public bool IsActive { get; set; }
        public bool Status { get; set; }
        public string? ProfilePicture { get; set; }
        public List<int>? Permissions { get; set; }
    }
}