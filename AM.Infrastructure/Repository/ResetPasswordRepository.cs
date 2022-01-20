using System;
using System.Linq;
using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.ResetPassword;
using AM.Domain.ResetPasswordAggregate;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class ResetPasswordRepository : RepositoryBase<long, ResetPassword>, IResetPasswordRepository
    {
        private readonly AMContext _amContext;
        public ResetPasswordRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public ResetPasswordModel GrabLink(string command)
        {
            return _amContext.UserResetPasswords.Select(x => new ResetPasswordModel
            {
                Guid = x.ResetUrl,
                UserId = x.UserId,
                CreationTime = x.CreationTime,
                ExpirationTime = x.ExprateDateTime
            }).AsNoTracking()
                .FirstOrDefault(x => x.Guid.ToString() == command);
        }
    }

}