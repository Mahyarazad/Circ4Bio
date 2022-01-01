using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.User
{
    public interface IUserApplication
    {
        List<UserViewModel> Search(UserSearchModel searchModel);
        OperationResult Register(RegisterUser command);
        OperationResult RegisterUser(RegisterUser command);
        OperationResult Edit(EditUser command);
        OperationResult ChangePassword(ChangePassword command);
        EditUser GetDetail(long Id);
        ChangePassword getDetailforChangePassword(long Id);
        OperationResult Login(EditUser command);
        void Logout();
    }
}