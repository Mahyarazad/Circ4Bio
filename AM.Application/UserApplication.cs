using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.User;
namespace AM.Application
{
    public class UserApplication : IUserApplication
    {
        public List<UserViewModel> Search(UserSearchModel searchModel)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Register(RegisterUser command)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult RegisterUser(RegisterUser command)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Edit(EditUser command)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            throw new System.NotImplementedException();
        }

        public EditUser GetDetail(long Id)
        {
            throw new System.NotImplementedException();
        }

        public ChangePassword getDetailforChangePassword(long Id)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Login(EditUser command)
        {
            throw new System.NotImplementedException();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }
    }
}
