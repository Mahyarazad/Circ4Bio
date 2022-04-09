using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.ResetPassword;

namespace AM.Application.Contracts.User
{
    public interface IUserApplication
    {
        Task<List<UserViewModel>> Search(UserSearchModel searchModel);
        Task<List<UserViewModel>> GetFullList();
        Task<List<RecipientViewModel>> GetUserListForListing(long id);
        OperationResult Register(RegisterUser command);
        Task<OperationResult> ChangePassword(ResetPasswordModel command);
        Task<OperationResult> ResetPassword(ResetPasswordModel command);
        Task<OperationResult> ActivateUser(string command);
        Task<OperationResult> SendActivationEmail(string command);
        Task<OperationResult> DeactivateUser(long Id);
        Task<OperationResult> AdminActivateUser(long Id);
        Task<OperationResult> AdminDectivateUser(long Id);
        Task<OperationResult> AdminActivateUserStatus(long Id);
        Task<OperationResult> AdminDectivateUserStatus(long Id);
        Task<OperationResult> EditByAdmin(EditUser command);
        Task<OperationResult> EditByUser(EditUser command);
        Task<EditUser> GetDetail(long Id);
        Task<EditUser> GetDetailByUsername(string username);
        Task<OperationResult> Login(EditUser command);
        void Logout();
        Task<List<Usertype>> GetUsertypes();
        Task<OperationResult> AddDeliveryLocation(CreateDeliveryLocation Command);
        Task<OperationResult> EditDeliveryLocation(CreateDeliveryLocation Command);
        Task<bool> RemoveDeliveryLocation(CreateDeliveryLocation Command);
        Task<List<CreateDeliveryLocation>> GetDeliveryLocationList(long userId);
        Task<CreateDeliveryLocation> GetDeliveryLocation(long UserId, long LocationId);
        Task<List<CreateDeliveryLocation>> GetDeliveryLocationDropDown(long userId);
    }
}