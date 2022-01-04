using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Role;
using Microsoft.EntityFrameworkCore;
using System;
using AM.Domain.RoleAggregate;


namespace AM.Infrastructure
{
    public class RoleRepository : RepositoryBase<int, Role>, IRoleRepository
    {
        private readonly AMContext _amContext;

        public RoleRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }


        public List<RoleViewModel> GetAll()
        {
            return _amContext.Roles.Select(x => new RoleViewModel
            {
                CreationTime = TruncateDateTime.TruncateToDefault(x.CreationTime).ToString(),
                Name = x.Name,
                Id = x.Id
            }).OrderByDescending(x => x.Id).ToList();
        }

        public EditRole GetDetail(int Id)
        {
            return _amContext.Roles.Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MapPermissions(x.Permissions)
            }).AsNoTracking().FirstOrDefault(x => x.Id == Id);
        }

        private static List<PermissionDTO> MapPermissions(List<Permission> permissions)
        {
            return permissions.Select(x => new PermissionDTO(x.Code, x.Name)).ToList();
        }
    }
}