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

        public OperationResult MarkAsRead(long Id)
        {
            var result = new OperationResult();
            if (_contactUsRepository.Exist(x => x.Id == Id))
            {
                var target = _contactUsRepository.Get(Id);
                target.MarkAsReed();
                _contactUsRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);

        }

        public List<ContactUsViewModel> GetContactUsMessages()
        {
            return _contactUsRepository.GetList().Select(x => new ContactUsViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime,
                FullName = x.FullName,
                Email = x.Email,
                Body = x.Body,
                Subject = x.Subject,
                Phone = x.Phone,
                IsReed = x.IsRead
            }).Where(x => !x.IsReed).OrderByDescending(x => x.Id).ToList();
        }

        public List<ContactUsViewModel> GetReadContactUsMessages()
        {
            return _contactUsRepository.GetList().Select(x => new ContactUsViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime,
                FullName = x.FullName,
                Email = x.Email,
                Body = x.Body,
                Subject = x.Subject,
                Phone = x.Phone,
                IsReed = x.IsRead
            }).Where(x => x.IsReed).OrderByDescending(x => x.Id).ToList();
        }

        public List<ContactUsViewModel> GetAllContactUsMessages()
        {
            return _contactUsRepository.GetList().Select(x => new ContactUsViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime,
                FullName = x.FullName,
                Email = x.Email,
                Body = x.Body,
                Subject = x.Subject,
                Phone = x.Phone,
                IsReed = x.IsRead
            }).OrderByDescending(x => x.Id).ToList();
        }

        public ContactUsViewModel GetSingleMessages(long Id)
        {
            var model = _contactUsRepository.Get(Id);
            return new ContactUsViewModel
            {
                Id = model.Id,
                CreationTime = model.CreationTime,
                FullName = model.FullName,
                Email = model.Email,
                Body = model.Body,
                Subject = model.Subject,
                Phone = model.Phone,
                IsReed = model.IsRead
            };
        }
    }
}