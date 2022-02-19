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
        public Task<List<UserViewModel>> Search(UserSearchModel searchModel);
        public Task<List<RecipientViewModel>> GetUserListForListing(long id);
        public Task<OperationResult> Register(RegisterUser command);
        public Task<OperationResult> ChangePassword(ResetPasswordModel command);
        public Task<OperationResult> ResetPassword(ResetPasswordModel command);
        public Task<OperationResult> ActivateUser(string command);
        public Task<OperationResult> SendActivationEmail(string command);
        public Task<OperationResult> AdminActivateUser(long Id);
        public Task<OperationResult> AdminDectivateUser(long Id);
        public Task<OperationResult> AdminActivateUserStatus(long Id);
        public Task<OperationResult> AdminDectivateUserStatus(long Id);
        public Task<OperationResult> EditByAdmin(EditUser command);
        public Task<OperationResult> EditByUser(EditUser command);
        public Task<EditUser> GetDetail(long Id);
        public Task<EditUser> GetDetailByUsername(string username);
        public Task<OperationResult> Login(EditUser command);
        void Logout();
        public Task<List<Usertype>> GetUsertypes();
        void AddDeliveryLocation(CreateDeliveryLocation Command);
        bool RemoveDeliveryLocation(CreateDeliveryLocation Command);
        public Task<List<CreateDeliveryLocation>> GetDeliveryLocationList(long userId);
        public Task<List<string>> GetDeliveryLocationDropDown(long userId);
    }
}