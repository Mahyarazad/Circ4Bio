using System;
using System.Threading.Tasks;
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
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmailService<EmailModel> _emailService;
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

        public Task<OperationResult> CreateResetPassword(string email)
        {

            var result = new OperationResult();

            if (!_userRepository.Exist(x => x.Email == email))
                return Task.FromResult(result.Failed(ApplicationMessage.UserNotExists));

            if (!_userRepository.GetDetailByEmail(email).Result.IsActive)
                return Task.FromResult(result.Failed(ApplicationMessage.UserNotActive));

            var userId = _userRepository.GetDetailByEmail(email).Id;
            var resetUrl = Guid.NewGuid();

            var request = _contextAccessor.HttpContext.Request;
            var emailModel = new EmailModel
            {
                EmailTemplate = 0,
                Title = ApplicationMessage.ResetPassword,
                Recipient = email,
                ResetPasswordLink =
                    $"{request.Scheme}://{request.Host}/Authentication/ResetPassword/{resetUrl.ToString()}".ToLower()
            };
            var emailServiceResult = _emailService.SendEmail(emailModel);


            if (emailServiceResult.IsSucceeded)
            {
                var passwordReset = new ResetPassword(userId, resetUrl);


                _resetPasswordRepository.Create(passwordReset);
                _resetPasswordRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
            else
            {
                return Task.FromResult(emailServiceResult);
            }

        }
        public Task<ResetPasswordModel> GetResetPasswordGuid(string guid)
        {
            var model = _resetPasswordRepository.GrabLink(guid).Result;
            if (model != null)
            {
                model.IsValid = DateTime.Now < model.ExpirationTime;
                return Task.FromResult(model);
            }

            return Task.FromResult(new ResetPasswordModel());
        }
    }
}