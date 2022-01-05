using AM.Application.Contracts.User;
using System.Collections.Generic;
using _0_Framework.Domain;

namespace AM.Domain.UserAggregate
{
    public interface IUserRepository : IRepository<long, User>
    {
        List<UserViewModel> Search(UserSearchModel searchModel);
        EditUser GetDetail(long Id);
        EditUser GetDetailByUser(string username);
        EditUser GetDetailByActivationUrl(string guid);
        ChangePassword getDetailforChangePassword(long Id);
    }
}