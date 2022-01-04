using AM.Application.Contracts.User;
using System.Collections.Generic;

namespace AM.Domain.UserAggregate
{
    public interface IUserRepository
    {
        List<UserViewModel> Search(UserSearchModel searchModel);
        EditUser GetDetail(long Id);
        EditUser GetDetailByUser(string username);
        ChangePassword getDetailforChangePassword(long Id);
    }
}