using System;
using AM.Application.Contracts.User;
using System.Collections.Generic;
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
        List<UserViewModel> Search(UserSearchModel searchModel);
        List<RecipientViewModel> GetUserListForListing(long id);
        EditUser GetDetail(long Id);
        EditUser GetDetailByUser(string username);
        ResendActivationEmail ResendActivationLink(string email);
        EditUser GetDetailByEmail(string email);
        EditUser GetDetailByActivationUrl(string guid);
        void AddDeliveryLocation(CreateDeliveryLocation Command);
        bool RemoveDeliveryLocation(CreateDeliveryLocation Command);
        List<CreateDeliveryLocation> GetDeliveryLocationList(long userId);
    }
}