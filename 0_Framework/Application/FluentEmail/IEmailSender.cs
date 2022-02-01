using System.Threading.Tasks;

namespace _0_Framework.Application.FluentEmail
{
    public interface IEmailSender
    {
        Task<bool> SendUsingTemplate(string to, string subject, EmailTemplate template, object model);

        public enum EmailTemplate
        {
            EmailConfirmation,
            PasswordReset
        }
    }
}