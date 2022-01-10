using System;
using _0_Framework.Domain;
using AM.Application.Contracts.ResetPassword;

namespace AM.Domain.UserAggregate
{
    public interface IResetPasswordRepository : IRepository<long, ResetPassword>
    {
        ResetPasswordModel GrabLink(string command);
    }
}