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
                        new PermissionDTO(UserPermission.ActivateUser, "Activate User"),
                        new PermissionDTO(UserPermission.DeactivateUser, "Deactivate User"),
                        new PermissionDTO(UserPermission.CreateRole, "Create Role"),
                        new PermissionDTO(UserPermission.EditRole, "Edit Role"),
                        new PermissionDTO(UserPermission.GetContactUsMEssages, "Get Contact Us Messages"),
                    }
                }
            };
        }
    }
}