using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Negotiate;
using AM.Domain.NegotiateAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class UserNegotiateRepository : IUserNegotiateRepository
    {
        private readonly AMContext _amContext;

        public UserNegotiateRepository(AMContext amContext)
        {
            _amContext = amContext;
        }

        public void Create(UserNegotiate Command)
        {
            _amContext.UserNegotiates.Add(Command);
        }

        public void Save()
        {
            _amContext.SaveChanges();
        }

        public List<UserNegotiate> GetAll()
        {
            return _amContext.Set<UserNegotiate>().AsNoTracking().ToList();
        }
    }
}