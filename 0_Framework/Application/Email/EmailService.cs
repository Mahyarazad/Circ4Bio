﻿using System;
using System.IO;
using FluentEmail.Core.Defaults;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Nancy;

namespace _0_Framework.Application.Email
{
    public class EmailService : IEmailService<EmailModel>
    {
        private readonly IConfiguration _configuration;
        public string? MailText { get; set; }
        public string? FilePath { get; set; }
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public OperationResult SendEmail(EmailModel model)
        {
            var result = new OperationResult();
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Circ4Bio", "admin@maahyarazad.ir"));

            message.To.Add(new MailboxAddress("Recipient", model.Recipient));
            message.Subject = model.Title;

            switch (model.EmailTemplate)
            {
                case 0:
                    {
                        FilePath = Directory.GetCurrentDirectory() + "\\Templates\\ResetPasswordTemplate.html";
                        StreamReader StreamReader = new StreamReader(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText.Replace("[link]", model.ResetPasswordLink);
                        break;
                    }

                case 1:
                    {
                        FilePath = Directory.GetCurrentDirectory() + "\\Templates\\AccountVerificationTemplate.html";
                        StreamReader StreamReader = new StreamReader(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText.Replace("[link]", model.AccountVerificationLink);
                        break;
                    }

                case 2:
                    {
                        FilePath = Directory.GetCurrentDirectory() + "\\Templates\\ProvideInformation.html";
                        StreamReader StreamReader = new StreamReader(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText
                            .Replace("[body]", model.Body)
                            .Replace("[body1]", model.Body1)
                            .Replace("[body2]", model.Body2)
                            .Replace("[body3]", model.Body3);
                    }
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
                client.Authenticate("admin@maahyarazad.ir", _configuration.GetSection("EmailPassword")["SecretKey"]);
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