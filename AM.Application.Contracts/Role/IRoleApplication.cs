using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Role
{
    public interface IRoleApplication
    {
        public Task<OperationResult> Create(CreateRole command);
        public Task<OperationResult> Edit(EditRole command);
        public Task<List<RoleViewModel>> GetAll();
        public Task<EditRole> GetRole(int Id);
    }
}