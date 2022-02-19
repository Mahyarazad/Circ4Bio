using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Role;
using AM.Domain.RoleAggregate;

namespace AM.Application
{
    public class RoleApplication : IRoleApplication
    {
        private readonly IRoleRepository _roleRepository;

        public RoleApplication(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public Task<OperationResult> Create(CreateRole command)
        {
            var result = new OperationResult();
            if (_roleRepository.Exist(x => x.Name == command.Name))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordExists));
            var role = new Role(command.Name, new List<Permission>());
            _roleRepository.Create(role);
            _roleRepository.SaveChanges();
            return Task.FromResult(result.Succeeded());

        }

        public Task<OperationResult> Edit(EditRole command)
        {
            var result = new OperationResult();
            var role = _roleRepository.Get(command.Id);
            if (_roleRepository.Exist(x => x.Id != command.Id && x.Name == command.Name))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordExists));

            var permissionList = new List<Permission>();
            if (command.Permissions != null)
                command.Permissions.ForEach(code => permissionList.Add(new Permission(code)));
            role.Result.Edit(command.Name, permissionList);
            _roleRepository.SaveChanges();
            return Task.FromResult(result.Succeeded());
        }

        public Task<List<RoleViewModel>> GetAll()
        {
            return _roleRepository.GetAll();
        }

        public Task<EditRole> GetRole(int Id)
        {
            return _roleRepository.GetDetail(Id);
        }
    }
}