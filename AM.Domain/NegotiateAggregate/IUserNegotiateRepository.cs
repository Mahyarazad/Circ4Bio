using System.Collections.Generic;

namespace AM.Domain.NegotiateAggregate
{
    public interface IUserNegotiateRepository
    {
        void Create(UserNegotiate Command);
        void Save();
        List<UserNegotiate> GetAll();
    }
}