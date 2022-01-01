using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.Role
{
    public interface IRoleApplication
    {
        OperationResult Create(CreateRole command);
        OperationResult Edit(EditRole command);
        List<RoleViewModel> GetAll();
        EditRole GetRole(int Id);
    }
}