using System;

namespace _0_Framework.Application
{
    public class RequirePermission : Attribute
    {
        public int Permission { get; set; }

        public RequirePermission(int permission)
        {
            Permission = permission;
        }
    }
}