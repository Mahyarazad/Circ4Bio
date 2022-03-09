using AM.Application.Contracts.Role;
using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Domain;

namespace AM.Domain.RoleAggregate
{
    public interface IRoleRepository : IRepository<int, Role>
    {
        List<RoleViewModel> GetAll();
        Task<RoleViewModel> GetDetailViewModel(int Id);
        Task<EditRole> GetDetail(int Id);

    }
}