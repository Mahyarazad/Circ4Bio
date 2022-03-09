using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Role;
using AM.Domain.RoleAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
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
            }).AsNoTracking()
                .OrderByDescending(x => x.Id).ToList();
        }

        public async Task<RoleViewModel> GetDetailViewModel(int Id)
        {
            return await _amContext.Roles.Select(x => new RoleViewModel
            {
                CreationTime = TruncateDateTime.TruncateToDefault(x.CreationTime).ToString(),
                Name = x.Name,
                Id = x.Id
            }).AsNoTracking()
                .OrderByDescending(x => x.Id).FirstAsync();
        }

        public async Task<EditRole> GetDetail(int Id)
        {
            return await _amContext.Roles.Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name,
                MappedPermissions = MapPermissions(x.Permissions)
            }).AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id);
        }

        private static List<PermissionDTO> MapPermissions(List<Permission> permissions)
        {
            return permissions.Select(x => new PermissionDTO(x.Code, x.Name)).ToList();
        }
    }
}