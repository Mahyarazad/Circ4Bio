using System;
using System.IO;
using FluentEmail.Core.Defaults;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace _0_Framework.Application.Email
{
    public class EmailService : IEmailService<EmailModel>
    {
        public OperationResult SendEmail(EmailModel model)
        {
            var result = new OperationResult();
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Circ4Bio", "admin@maahyarazad.ir"));

            message.To.Add(new MailboxAddress("Recipient", model.Recipient));
            message.Subject = model.Title;
            string MailText = null;
            switch (model.EmailTemplate)
            {
                case 0:
                    string FilePath = Directory.GetCurrentDirectory() + "\\Templates\\ResetPasswordTemplate.html";
                    StreamReader str = new StreamReader(FilePath);
                    MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[link]", model.ResetPasswordLink);
                    break;
                case 1:
                    FilePath = Directory.GetCurrentDirectory() + "\\Templates\\AccountVerificationTemplate.html";
                    str = new StreamReader(FilePath);
                    MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[link]", model.AccountVerificationLink);
                    break;
                case 2:
                    FilePath = Directory.GetCurrentDirectory() + "\\Templates\\ProvideInformation.html";
                    str = new StreamReader(FilePath);
                    MailText = str.ReadToEnd();
                    str.Close();
                    MailText = MailText.Replace("[link]", model.AccountVerificationLink)
                        .Replace("[body]", model.Body)
                        .Replace("[body1]", model.Body1)
                        .Replace("[body2]", model.Body2)
                        .Replace("[body3]", model.Body3);

                    break;
            }


            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = MailText
            };
            // var bodyBuilder = new BodyBuilder
            // {
            //     HtmlBody = $"{messageBody}"
            // };
            //
            // message.Body = bodyBuilder.ToMessageBody();

            var client = new SmtpClient();

            try
            {
                client.Connect(host: "mail.maahyarazad.ir", port: 587, SecureSocketOptions.None);
                client.Authenticate("admin@maahyarazad.ir", "g5kv,foljqIpnmacsbhr");
                client.Send(message);
                client.Disconnect(true);
                client.Dispose();
                return result.Succeeded();
            }
            catch (Exception e)
            {
                return result.Failed(e.Message);
            }

        }

    }
}