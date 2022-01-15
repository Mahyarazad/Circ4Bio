using System;
using AM.Application.Contracts.User;
using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.ResetPassword;

namespace AM.Domain.UserAggregate
{
    public interface IUserRepository : IRepository<long, User>
    {
        List<UserViewModel> Search(UserSearchModel searchModel);
        EditUser GetDetail(long Id);
        EditUser GetDetailByUser(string username);
        ResendActivationEmail ResendActivationLink(string email);
        EditUser GetDetailByEmail(string email);
        EditUser GetDetailByActivationUrl(string guid);
    }
}