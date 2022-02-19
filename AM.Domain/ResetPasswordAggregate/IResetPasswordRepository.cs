using System.Threading.Tasks;
using _0_Framework.Domain;
using AM.Application.Contracts.ResetPassword;

namespace AM.Domain.ResetPasswordAggregate
{
    public interface IResetPasswordRepository : IRepository<long, ResetPassword>
    {
        Task<ResetPasswordModel> GrabLink(string command);
    }
}