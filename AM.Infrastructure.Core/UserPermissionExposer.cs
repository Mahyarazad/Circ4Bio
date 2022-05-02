using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace AM.Infrastructure.Core
{
    public class UserPermissionExposer : IPermissionExposer
    {
        public Dictionary<string, List<PermissionDTO>> Exposer()
        {
            return new Dictionary<string, List<PermissionDTO>>
            {
                {
                    "Account Management", new List<PermissionDTO>
                    {
                        new PermissionDTO(UserPermission.RegisterUser, "Register User"),
                        new PermissionDTO(UserPermission.VerifyUserEmail, "Verify User Email"),
                        new PermissionDTO(UserPermission.DisableUserEmail, "Disable User Email"),
                        new PermissionDTO(UserPermission.ActivateUserStatus, "Activate User Status"),
                        new PermissionDTO(UserPermission.DisableUserStatus, "Disable User Status"),
                        new PermissionDTO(UserPermission.CreateRole, "Create Role"),
                        new PermissionDTO(UserPermission.EditRole, "Edit Role"),
                        new PermissionDTO(UserPermission.GetContactUsMessages, "Get Contact Us Messages"),
                        new PermissionDTO(UserPermission.CreateBlogPost, "Create Blog Post"),
                        new PermissionDTO(UserPermission.EditBlogPost, "Edit Blog Post"),
                    }
                }
            };
        }
    }
}