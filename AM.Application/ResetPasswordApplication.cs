using System;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using _0_Framework.Application.FluentEmail;
using AM.Application.Contracts.ResetPassword;
using AM.Domain.ResetPasswordAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using Nancy.ViewEngines.SuperSimpleViewEngine;

namespace AM.Application
{
    public class ResetPasswordApplication : IResetPasswordApplication
    {
        private readonly IEmailService<EmailModel> _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IResetPasswordRepository _resetPasswordRepository;

        public ResetPasswordApplication(IResetPasswordRepository resetPasswordRepository,
            IUserRepository userRepository, IEmailService<EmailModel> emailService,
            IHttpContextAccessor contextAccessor)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
            _resetPasswordRepository = resetPasswordRepository;
        }

        public OperationResult CreateResetPassword(string email)
        {

            var result = new OperationResult();

            if (!_userRepository.Exist(x => x.Email == email))
                return result.Failed(ApplicationMessage.UserNotExists);

            if (!_userRepository.GetDetailByEmail(email).IsActive)
                return result.Failed(ApplicationMessage.UserNotActive);

            var userId = _userRepository.GetDetailByEmail(email).Id;
            var resetUrl = Guid.NewGuid();

            var request = _contextAccessor.HttpContext.Request;
            var emailModel = new EmailModel
            {
                EmailTemplate = 0,
                Title = ApplicationMessage.ResetPassword,
                Recipient = email,
                ResetPasswordLink =
                    $"https://{request.Host}/Authentication/ResetPassword/{resetUrl.ToString()}".ToLower()
            };
            var emailServiceResult = _emailService.SendEmail(emailModel);


            if (emailServiceResult.IsSucceeded)
            {
                var passwordReset = new ResetPassword(userId, resetUrl);


                _resetPasswordRepository.Create(passwordReset);
                _resetPasswordRepository.SaveChanges();
                return result.Succeeded();
            }
            else
            {
                return emailServiceResult;
            }

        }

        public ResetPasswordModel GetResetPasswordGuid(string guid)
        {
            var model = _resetPasswordRepository.GrabLink(guid);
            if (model != null)
            {
                model.IsValid = DateTime.Now < model.ExpirationTime;
                return model;
            }

            return new ResetPasswordModel();
        }
    }
}