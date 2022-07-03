using System;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

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

            message.From.Add(new MailboxAddress("Circ4Bio", _configuration.GetSection("EmailService")["AdminEmail"]));

            message.To.Add(new MailboxAddress("Recipient", model.Recipient));
            message.Subject = model.Title;

            switch (model.EmailTemplate)
            {
                case EmailType.ResetPassword:
                    {
                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                                   + "\\wwwroot\\Email Templates\\ResetPasswordTemplate.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //+ "\\wwwroot\\Email Templates\\ResetPassword.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText.Replace("[link]", model.ResetPasswordLink)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }

                case EmailType.AccountVerificationLink:
                    {

                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                        + "\\wwwroot\\Email Templates\\AccountVerificationTemplate.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //+ "\\wwwroot\\Email Templates\\AccountVerificationTemplate.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText.Replace("[link]", model.AccountVerificationLink)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }

                case EmailType.ProvideInformation:
                    {
                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                                   + "\\wwwroot\\Email Templates\\ProvideInformation.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //+ "\\wwwroot\\Email Templates\\ProvideInformation.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText
                            .Replace("[body]", model.Body)
                            .Replace("[body1]", model.Body1)
                            .Replace("[body2]", model.Body2)
                            .Replace("[body3]", model.Body3)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }
                case EmailType.NegotiationOpened:
                    {
                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                                   + "\\wwwroot\\Email Templates\\NegotiationOpened.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //+ "\\wwwroot\\Email Templates\\NegotiationOpened.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText
                            .Replace("[body]", model.Body)
                            .Replace("[body1]", model.Body1)
                            .Replace("[body2]", model.Body2)
                            .Replace("[body3]", model.Body3)
                            .Replace("[body4]", model.Body4)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }
                case EmailType.QuotationCreated:
                    {
                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                                   + "\\wwwroot\\Email Templates\\QuotationCreated.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //+ "\\wwwroot\\Email Templates\\QuotationCreated.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText
                            .Replace("[body]", model.Body)
                            .Replace("[body1]", model.Body1)
                            .Replace("[body2]", model.Body2)
                            .Replace("[body3]", model.Body3)
                            .Replace("[body4]", model.Body4)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }
                case EmailType.ActiveDeal:
                    {
                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                                   + "\\wwwroot\\Email Templates\\ActiveDeal.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //+ "\\wwwroot\\Email Templates\\ActiveDeal.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText
                            .Replace("[body]", model.Body)
                            .Replace("[body1]", model.Body1)
                            .Replace("[body2]", model.Body2)
                            .Replace("[body3]", model.Body3)
                            .Replace("[body4]", model.Body4)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }
                case EmailType.SuccessfulPayment:
                    {
                        FilePath = AppDomain.CurrentDomain.BaseDirectory
                                   + "\\wwwroot\\Email Templates\\SuccessfulPayment.html";
                        //FilePath = "C:\\Users\\mhyri\\OneDrive\\Desktop\\Circ4Bio\\Circ4Bio\\ServiceHost"
                        //       + "\\wwwroot\\Email Templates\\SuccessfulPayment.html";
                        StreamReader StreamReader = new(FilePath);
                        MailText = StreamReader.ReadToEnd();
                        StreamReader.Close();
                        MailText = MailText
                            .Replace("[body]", model.Body)
                            .Replace("[body1]", model.Body1)
                            .Replace("[body2]", model.Body2)
                            .Replace("[body3]", model.Body3)
                            .Replace("[year]", DateTime.Today.Year.ToString())
                            .Replace("[title]", model.Title);
                        break;
                    }
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
                client.Connect(host: _configuration.GetSection("EmailService")["Host"]
                    , port: Convert.ToInt32(_configuration.GetSection("EmailService")["Port"])
                    , SecureSocketOptions.SslOnConnect);
                client.Authenticate(_configuration.GetSection("EmailService")["User"]
                    , _configuration.GetSection("EmailService")["Password"]);
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