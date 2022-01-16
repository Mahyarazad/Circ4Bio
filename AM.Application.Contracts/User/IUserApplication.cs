using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.ResetPassword;

namespace AM.Application.Contracts.User
{
    public interface IUserApplication
    {
        List<UserViewModel> Search(UserSearchModel searchModel);
        OperationResult Register(RegisterUser command);
        OperationResult RegisterUser(RegisterUser command);
        OperationResult ActivateUser(string command);
        OperationResult AdminActivateUser(long Id);
        OperationResult SendActivationEmail(string command);
        OperationResult AdminDectivateUser(long Id);
        OperationResult EditByAdmin(EditUser command);
        OperationResult EditByUser(EditUser command);
        OperationResult ChangePassword(ResetPasswordModel command);
        EditUser GetDetail(long Id);
        ChangePassword getDetailforChangePassword(long Id);
        OperationResult ResetPassword(ResetPasswordModel command);
        OperationResult Login(EditUser command);
        void Logout();
        List<Usertype> GetUsertypes();
    }
}