using AM.Application.Contracts.Role;
using System.Collections.Generic;
using _0_Framework.Domain;

namespace AM.Domain.RoleAggregate
{
    public interface IRoleRepository : IRepository<int, Role>
    {
        List<RoleViewModel> GetAll();
        EditRole GetDetail(int Id);

    }
}