using _0_Framework.Application;

namespace AM.Application.Contracts.ResetPassword
{
    public interface IResetPasswordApplication
    {
        OperationResult CreateResetPassword(string email);
        ResetPasswordModel GetResetPasswordGuid(string guid);

    }
}