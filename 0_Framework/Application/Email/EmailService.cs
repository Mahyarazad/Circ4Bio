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

            message.From.Add(new MailboxAddress("Admin", "admin@maahyarazad.ir"));

            message.To.Add(new MailboxAddress("Recipient", recipient));
            message.Subject = title;
            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"<p1>{messageBody}<p1>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();
            client.Connect(host: "mail.maahyarazad.ir", port: 587, SecureSocketOptions.None);
            client.Authenticate("admin@maahyarazad.ir", "g5kv,foljqIpnmacsbhr");
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}