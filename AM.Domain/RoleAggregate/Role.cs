using _0_Framework.Domain;
using System.Collections.Generic;
using AM.Domain.UserAggregate;

namespace AM.Domain.RoleAggregate
{
    public class Role : BaseEntity<int>
    {
        protected Role()
        {

        }
        public Role(string name, List<Permission> permissions)
        {
            Name = name;
            Accounts = new List<User>();
            Permissions = permissions;
        }

        public string Name { get; private set; }
        public List<User> Accounts { get; private set; }
        public List<Permission> Permissions { get; private set; }

        public void Edit(string name, List<Permission> permissions)
        {
            Name = name;
            Permissions = permissions;
        }
    }
}
