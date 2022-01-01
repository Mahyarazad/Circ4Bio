using System.Collections.Generic;
using _0_Framework.Infrastructure;

namespace AM.Application.Contracts.Role
{
    public class CreateRole
    {
        public string? Name { get; set; }
        public List<int>? Permissions { get; set; }
    }

    public class EditRole : CreateRole
    {
        public int Id { get; set; }
        public List<PermissionDTO>? MappedPermissions { get; set; }
    }

    public class RoleViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? CreationTime { get; set; }
    }
}