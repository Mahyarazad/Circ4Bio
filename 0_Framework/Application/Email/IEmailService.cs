namespace _0_Framework.Application.Email
{
    public interface IEmailService
    {
        OperationResult SendEmail(string title, string messageBody, string recipient);
    }
}