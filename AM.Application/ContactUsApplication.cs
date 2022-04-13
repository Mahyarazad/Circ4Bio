using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.ContactUs;
using AM.Domain.ContactUsAggregate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AM.Application
{
    public class ContactUsApplication : IContactUsApplication
    {
        private readonly IConfiguration _configuration;
        private readonly IContactUsRepository _contactUsRepository;
        private readonly IEmailService<EmailModel> _emailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ContactUsApplication(IContactUsRepository contactUsRepository, IConfiguration configuration,
            IEmailService<EmailModel> emailService, IHttpContextAccessor contextAccessor)
        {
            _emailService = emailService;
            _configuration = configuration;
            _contextAccessor = contextAccessor;
            _contactUsRepository = contactUsRepository;
        }

        public Task<OperationResult> CreateMessage(CreateMessage command)
        {
            var result = new OperationResult();
            if (!string.IsNullOrWhiteSpace(command.Email)
                && !string.IsNullOrWhiteSpace(command.Body)
                && !string.IsNullOrWhiteSpace(command.FullName))
            {

                var request = _contextAccessor.HttpContext.Request;
                var emailModel = new EmailModel
                {
                    EmailTemplate = EmailType.ProvideInformation,
                    Subject = command.Subject,
                    Fullname = command.FullName,
                    Email = command.Email,
                    Phone = command.Phone,
                    Body1 = command.Body,
                    Recipient = _configuration.GetSection("EmailService")["AdminEmail"],
                    Title = command.Subject,
                };
                var emailServiceResult = _emailService.SendEmail(emailModel);

                if (emailServiceResult.IsSucceeded)
                {
                    var message = new ContactUs(command.FullName, command.Email, command.Body, command.Subject, command.Phone);
                    _contactUsRepository.Create(message);
                    _contactUsRepository.SaveChanges();
                    return Task.FromResult(result.Succeeded(ApplicationMessage.ContactUsSuccess));
                }
                else
                {
                    return Task.FromResult(emailServiceResult);
                }
            }
            return Task.FromResult(result.Failed(ApplicationMessage.SomethingWentWrong));
        }

        public async Task<OperationResult> MarkAsRead(long Id)
        {
            var result = new OperationResult();
            if (_contactUsRepository.Exist(x => x.Id == Id))
            {
                var target = await _contactUsRepository.Get(Id);
                target.MarkAsReed();
                _contactUsRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);

        }

        public async Task<List<ContactUsViewModel>> GetContactUsMessages()
        {
            var query = await _contactUsRepository.GetList();
            var result = query.Select(x => new ContactUsViewModel
            {
                Id = x.Id,
                CreationTime = x.CreationTime,
                FullName = x.FullName,
                Email = x.Email,
                Body = x.Body,
                Subject = x.Subject,
                Phone = x.Phone,
                IsReed = x.IsRead
            }).Where(x => !x.IsReed)
                .OrderByDescending(x => x.Id).ToList();
            return result;
        }

        public async Task<List<ContactUsViewModel>> GetReadContactUsMessages()
        {
            var query = await _contactUsRepository.GetList();
            var result = query.Select(x => new ContactUsViewModel
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
            return result;
        }

        public async Task<List<ContactUsViewModel>> GetAllContactUsMessages()
        {
            var query = await _contactUsRepository.GetList();
            var result = query.Select(x => new ContactUsViewModel
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
            return result;
        }

        public async Task<ContactUsViewModel> GetSingleMessages(long Id)
        {
            var model = await _contactUsRepository.Get(Id);
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