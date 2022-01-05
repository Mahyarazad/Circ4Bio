using System;
using System.Collections.Generic;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.User;
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;

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

        public UserApplication(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAutenticateHelper authenticateHelper,
            IFileUploader fileUploader,
            IRoleRepository roleRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _fileUploader = fileUploader;
            _autenticateHelper = authenticateHelper;
            _roleRepository = roleRepository;
            _emailService = emailService;
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

            _emailService.SendEmail(ApplicationMessage.AccountVerification, activationGuid.ToString(), command.Email);
            var user = new User(command.Email, password, command.Type, activationGuid, command.Type);



            _userRepository.Create(user);
            _userRepository.SaveChanges();
            return result.Succeeded();
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
                user.ActivateUser();
                _userRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);

        }

        public OperationResult Edit(EditUser command)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult ChangePassword(ChangePassword command)
        {
            throw new System.NotImplementedException();
        }

        public EditUser GetDetail(long Id)
        {
            throw new System.NotImplementedException();
        }

        public ChangePassword getDetailforChangePassword(long Id)
        {
            throw new System.NotImplementedException();
        }

        public OperationResult Login(EditUser command)
        {
            throw new System.NotImplementedException();
        }

        public void Logout()
        {
            throw new System.NotImplementedException();
        }

        public List<Usertype> GetUsertypes()
        {
            var type = new Usertype(0, "Default");
            return type.GetUserTypeList();
        }
    }
}
