using System.Collections.Generic;

namespace _0_Framework.Application
{
    public class AuthViewModel
    {
        public AuthViewModel()
        {

        }

        public AuthViewModel(long id, string username, string fullname
            , string roleId, string profilePicture, List<int> permissions)
        {
            Id = id;
            Username = username;
            Fullname = fullname;
            RoleId = roleId;
            ProfilePicture = profilePicture;
            Permissions = permissions;

        }
        public long Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string RoleId { get; set; }
        public string ProfilePicture { get; set; }
        public List<int> Permissions { get; set; }
    }
}