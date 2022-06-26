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
                        new PermissionDTO(UserPermission.GetUserList, "Get User List"),
                        new PermissionDTO(UserPermission.RegisterUser, "Register User"),
                        new PermissionDTO(UserPermission.EditUser, "Edit User"),
                        new PermissionDTO(UserPermission.VerifyUserEmail, "Verify User Email"),
                        new PermissionDTO(UserPermission.DisableUserEmail, "Disable User Email"),
                        new PermissionDTO(UserPermission.ActivateUserStatus, "Activate User Status"),
                        new PermissionDTO(UserPermission.DisableUserStatus, "Disable User Status"),
                        new PermissionDTO(UserPermission.GetRole, "Get Roles"),
                        new PermissionDTO(UserPermission.CreateRole, "Create Role"),
                        new PermissionDTO(UserPermission.EditRole, "Edit Role"),
                    }
                },
                {
                    "Blog Management", new List<PermissionDTO>
                    {
                        new PermissionDTO(UserPermission.CreateBlogPost, "Create Blog Post"),
                        new PermissionDTO(UserPermission.EditBlogPost, "Edit Blog Post"),
                        new PermissionDTO(UserPermission.GetBlogPost, "Get Blog Post"),
                    }
                },
                {
                    "Contact Us Management", new List<PermissionDTO>
                    {
                        new PermissionDTO(UserPermission.GetContactUsMessages, "Get Contact Us Messages"),
                        new PermissionDTO(UserPermission.MarkAsReedMessages, "Mark as Reed"),
                    }
                },
                {
                    "Nace Management", new List<PermissionDTO>
                    {
                        new PermissionDTO(UserPermission.GetNace, "Get NACE"),
                        new PermissionDTO(UserPermission.RegisterNace, "Register NACE"),
                        new PermissionDTO(UserPermission.DeleteNace, "Delete NACE"),
                        new PermissionDTO(UserPermission.EditNace, "Edit NACE"),
                        new PermissionDTO(UserPermission.ViewNace, "View NACE"),
                    }
                },
                {
                    "Listing Management", new List<PermissionDTO>
                    {
                        new PermissionDTO(UserPermission.CreateListing, "Create Listing"),
                        new PermissionDTO(UserPermission.EditListing, "Edit Listing"),
                        new PermissionDTO(UserPermission.DeleteListing, "Delete Listing"),
                        new PermissionDTO(UserPermission.GetListing, "Get Listing"),
                        new PermissionDTO(UserPermission.AddNaceData, "Add Nace Data"),
                        new PermissionDTO(UserPermission.EditNaceData, "Edit Nace Data"),
                        new PermissionDTO(UserPermission.DeleteNaceData, "Delete Nace Data"),
                        new PermissionDTO(UserPermission.CreateNegotiation, "Create Negotiation"),
                    }
                }
            };

        }
    }
}