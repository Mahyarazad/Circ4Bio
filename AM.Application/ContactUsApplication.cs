using System.Collections.Generic;
using System.Linq;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.ContactUs;
using AM.Domain.ContactUsAggregate;
using Microsoft.AspNetCore.Http;

namespace AM.Application
{
    public class ContactUsApplication : IContactUsApplication
    {
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ContactUsApplication(IContactUsRepository contactUsRepository, IEmailService emailService, IHttpContextAccessor contextAccessor)
        {
            _contactUsRepository = contactUsRepository;
            _emailService = emailService;
            _contextAccessor = contextAccessor;
        }

        public OperationResult CreateMessage(CreateMessage command)
        {
            var result = new OperationResult();
            if (!string.IsNullOrWhiteSpace(command.Email)
                && !string.IsNullOrWhiteSpace(command.Body)
                && !string.IsNullOrWhiteSpace(command.FullName))
            {

                var request = _contextAccessor.HttpContext.Request;
                var emailServiceResult = _emailService.SendEmail(command.Subject
                    , $"<h3>Inquirer: {command.FullName} </h3>" +
                      $"<h3>Inquirer Email: {command.Email} </h3>" +
                      $"<h3>Inquirer Phone: {command.Phone} </h3>" +
                      $"<p>{command.Body}</p>"
                    , "admin@maahyarazad.ir");

                if (emailServiceResult.IsSucceeded)
                {
                    var message = new ContactUs(command.FullName, command.Email, command.Body, command.Subject, command.Phone);
                    _contactUsRepository.Create(message);
                    _contactUsRepository.SaveChanges();
                    return result.Succeeded(ApplicationMessage.ContactUsSuccess);
                }
                else
                {
                    return emailServiceResult;
                }
            }
            return result.Failed(ApplicationMessage.SomethingWentWrong);
        }

        public List<ContactUsViewModel> GetContactUsMessages()
        {
            return _contactUsRepository.GetList().Select(x => new ContactUsViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime,
                FullName = x.FullName,
                Email = x.Email,
                Body = x.Body
            }).OrderByDescending(x => x.Id).ToList();
        }
    }
}