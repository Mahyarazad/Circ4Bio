using System.Collections.Generic;
using System.Threading.Tasks;

namespace AM.Domain.NegotiateAggregate
{
    public interface IUserNegotiateRepository
    {
        void Create(UserNegotiate Command);
        void Save();
        Task<List<UserNegotiate>> GetAll();
    }
}