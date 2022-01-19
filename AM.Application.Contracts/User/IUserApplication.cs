using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.ResetPassword;

namespace AM.Application.Contracts.User
{
    public interface IUserApplication
    {
        List<UserViewModel> Search(UserSearchModel searchModel);
        OperationResult Register(RegisterUser command);
        OperationResult ChangePassword(ResetPasswordModel command);
        OperationResult ResetPassword(ResetPasswordModel command);
        OperationResult ActivateUser(string command);
        OperationResult SendActivationEmail(string command);
        OperationResult AdminActivateUser(long Id);
        OperationResult AdminDectivateUser(long Id);
        OperationResult AdminActivateUserStatus(long Id);
        OperationResult AdminDectivateUserStatus(long Id);
        OperationResult EditByAdmin(EditUser command);
        OperationResult EditByUser(EditUser command);
        EditUser GetDetail(long Id);
        OperationResult Login(EditUser command);
        void Logout();
        List<Usertype> GetUsertypes();
    }
}