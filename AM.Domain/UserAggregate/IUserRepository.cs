using System;
using AM.Application.Contracts.User;
using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.ResetPassword;
using AM.Domain.NotificationAggregate;

namespace AM.Domain.UserAggregate
{
    public interface IUserRepository : IRepository<long, User>
    {
        public Task<List<UserViewModel>> Search(UserSearchModel searchModel);
        public Task<List<RecipientViewModel>> GetUserListForListing(long id);
        public Task<EditUser> GetDetail(long Id);
        public Task<EditUser> GetDetailByUser(string username);
        public Task<ResendActivationEmail> ResendActivationLink(string email);
        public Task<EditUser> GetDetailByEmail(string email);
        public Task<EditUser> GetDetailByActivationUrl(string guid);
        void AddDeliveryLocation(CreateDeliveryLocation Command);
        bool RemoveDeliveryLocation(CreateDeliveryLocation Command);
        public Task<List<CreateDeliveryLocation>> GetDeliveryLocationList(long userId);
        public Task<List<string>> GetDeliveryLocationDropDown(long userId);
    }
}