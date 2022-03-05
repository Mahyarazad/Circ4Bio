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
        Task<List<UserViewModel>> Search(UserSearchModel searchModel);
        Task<List<RecipientViewModel>> GetUserListForListing(long id);
        Task<EditUser> GetDetail(long Id);
        Task<EditUser> GetDetailByUser(string username);
        Task<ResendActivationEmail> ResendActivationLink(string email);
        Task<EditUser> GetDetailByEmail(string email);
        Task<EditUser> GetDetailByActivationUrl(string guid);
        Task<OperationResult> AddDeliveryLocation(CreateDeliveryLocation Command);
        Task<OperationResult> EditDeliveryLocation(CreateDeliveryLocation Command);
        Task<bool> RemoveDeliveryLocation(CreateDeliveryLocation Command);
        Task<List<CreateDeliveryLocation>> GetDeliveryLocationList(long userId);
        Task<CreateDeliveryLocation> GetDeliveryLocation(long userId, long locationId);
        Task<List<CreateDeliveryLocation>> GetDeliveryLocationDropDown(long userId);
        Task<bool> CheckDeliveryLocationName(string Name, long userId);
    }
}