using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using Nancy.Json;
using Org.BouncyCastle.Asn1.Ocsp;

namespace AM.Application
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IFileUploader _fileUploader;
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IResetPasswordApplication _resetPasswordApplication;

        public UserApplication(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAutenticateHelper authenticateHelper,
            IFileUploader fileUploader,
            IRoleRepository roleRepository,
            IHttpContextAccessor contextAccessor,
            IResetPasswordApplication resetPasswordApplication,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _autenticateHelper = authenticateHelper;
            _roleRepository = roleRepository;
            _emailService = emailService;
            _contextAccessor = contextAccessor;
            _resetPasswordApplication = resetPasswordApplication;
        }

        public List<UserViewModel> Search(UserSearchModel searchModel)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Register(RegisterUser command)
        {
            var result = new OperationResult();
            if (_userRepository.Exist(x => x.Email == command.Email))
                return result.Failed(ApplicationMessage.RecordExists);

            // var profilePicture =
            //     _fileUploader.Uploader(command.ProfilePicture, "\\ProfilePicture\\", command.UserId);
            // if (command.ProfilePicture == null)
            //     profilePicture = "DefaultProfile.png";
            var password = _passwordHasher.Hash(command.Password);
            var activationGuid = Guid.NewGuid();
            var request = _contextAccessor.HttpContext.Request;
            var emailServiceResult = _emailService.SendEmail(ApplicationMessage.AccountVerification
                , $"https://{request.Host}/Authentication/ActivateUser/{activationGuid.ToString()}"
                , command.Email);

            if (emailServiceResult.IsSucceeded)
            {
                var user = new User(command.Email, password, command.Type, activationGuid, command.Type);
                _userRepository.Create(user);
                _userRepository.SaveChanges();
                return result.Succeeded();
            }
            else
            {
                return emailServiceResult;
            }
        }

        public OperationResult RegisterUser(RegisterUser command)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult ActivateUser(string command)
        {
            var result = new OperationResult();
            if (!string.IsNullOrWhiteSpace(command))
            {
                var user = _userRepository.Get(_userRepository.GetDetailByActivationUrl(command).Id);
                if (user != null)
                {
                    user.ActivateUser();
                    _userRepository.SaveChanges();
                    return result.Succeeded();
                }
                return result.Failed(ApplicationMessage.ActivationUrlError);
            }
            return result.Failed(ApplicationMessage.SomethingWentWrong);

        }

        public OperationResult Edit(EditUser command)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult ChangePassword(ResetPasswordModel command)
        {
            var result = new OperationResult();
            var user = _userRepository.Get(command.UserId);

            return result.Succeeded();
        }


        public EditUser GetDetail(long Id)
        {
            var user = _userRepository.GetDetail(Id);
            return user;
        }

        public ChangePassword getDetailforChangePassword(long Id)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult ResetPassword(ResetPasswordModel command)
        {
            var result = new OperationResult();
            var user = _userRepository.Get(command.UserId);
            var password = _passwordHasher.Hash(command.Password);
            user.ChangePassword(password);
            _userRepository.SaveChanges();
            return result.Succeeded(ApplicationMessage.ResetPasswordSuccess);

        }

        public OperationResult Login(EditUser command)
        {
            var result = new OperationResult();
            var user = _userRepository.GetDetailByEmail(command.Email);

            if (command.RememberMe)
            {
                var cookieOptions = new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true, //<- there
                    Expires = DateTime.Now.AddDays(15),
                };
                var userToRemember = new RememberMe
                {
                    Email = command.Email,
                };
                _contextAccessor.HttpContext.Response.Cookies
                    .Append("user-token", new JavaScriptSerializer().Serialize(userToRemember), cookieOptions);
            }
            else
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete("user-token");
            }

            if (!user.IsActive)
                return result.Failed(ApplicationMessage.UserNotActive);
            if (user == null)
                return result.Failed(ApplicationMessage.UserNotExists);

            var (verified, needsUpgrade) = _passwordHasher.Check(user.Password, command.Password);
            if (!verified)
                return result.Failed(ApplicationMessage.WrongPassword);

            var permissions = _roleRepository.GetDetail(user.RoleId)
                .MappedPermissions
                .Select(x => x.Code)
                .ToList();

            var authModel = new AuthViewModel(user.Id, user.Email, user.FullName,
                user.RoleId.ToString(), command.RememberMe, user.PictureString, permissions, command.Password);
            _autenticateHelper.Login(authModel);
            return result.Succeeded();
        }

        public void Logout()
        {
            _autenticateHelper.Logout();
        }

        public List<Usertype> GetUsertypes()
        {
            var type = new Usertype(0, "Default");
            return type.GetUserTypeList();
        }
    }
}
