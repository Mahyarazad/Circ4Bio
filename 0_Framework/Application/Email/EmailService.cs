using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace _0_Framework.Application.Email
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string title, string messageBody, string recipient)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Admin", "admin@domainName.com"));

            message.To.Add(new MailboxAddress("Recipient", recipient));
            message.Subject = title;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<h1>{messageBody}<h1>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect(host: "mail.domainName.com", port: 587, SecureSocketOptions.None);
            client.Authenticate("admin@domainName.com", "password");
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}