using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.ResetPassword
{
    public interface IResetPasswordApplication
    {
        Task<OperationResult> CreateResetPassword(string email);
        Task<ResetPasswordModel> GetResetPasswordGuid(string guid);

    }
}