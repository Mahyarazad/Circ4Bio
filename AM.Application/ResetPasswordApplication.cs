using System;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.ResetPassword;
using AM.Domain.ResetPasswordAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;

namespace AM.Application
{
    public class ResetPasswordApplication : IResetPasswordApplication
    {
        private readonly IResetPasswordRepository _resetPasswordRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _contextAccessor;

        public ResetPasswordApplication(IResetPasswordRepository resetPasswordRepository,
            IUserRepository userRepository, IEmailService emailService,
            IHttpContextAccessor contextAccessor)
        {
            _resetPasswordRepository = resetPasswordRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
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
            var emailServiceResult = _emailService.SendEmail("Reset Password"
                , $"https://{request.Host}/Authentication/ResetPassword/{resetUrl.ToString()}"
                , email);
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