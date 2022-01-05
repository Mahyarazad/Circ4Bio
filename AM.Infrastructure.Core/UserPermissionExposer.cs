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
                    "Users", new List<PermissionDTO>
                    {
                        new PermissionDTO(UserPermission.RegisterUser, "Register User"),
                    }
                }
            };
        }
    }
}