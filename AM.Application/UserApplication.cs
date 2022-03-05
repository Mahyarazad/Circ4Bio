using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application.Contracts.Listing;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.ResetPassword;
using AM.Application.Contracts.User;
using AM.Domain.NotificationAggregate;
using AM.Domain.RoleAggregate;
using AM.Domain.UserAggregate;
using Microsoft.AspNetCore.Http;
using Nancy.Json;


namespace AM.Application
{
    public class UserApplication : IUserApplication
    {
        private readonly IFileUploader _fileUploader;
        private readonly IEmailService<EmailModel> _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRoleRepository _roleRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly IRecipientRepository _recipientRepository;
        private readonly INotificationApplication _notificationApplication;
        private readonly IResetPasswordApplication _resetPasswordApplication;

        public UserApplication(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IAuthenticateHelper authenticateHelper,
            IFileUploader fileUploader,
            IRoleRepository roleRepository,
            IHttpContextAccessor contextAccessor,
            IRecipientRepository recipientRepository,
            IResetPasswordApplication resetPasswordApplication,
            INotificationApplication notificationApplication,
            IEmailService<EmailModel> emailService)
        {
            _fileUploader = fileUploader;
            _emailService = emailService;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _roleRepository = roleRepository;
            _contextAccessor = contextAccessor;
            _authenticateHelper = authenticateHelper;
            _recipientRepository = recipientRepository;
            _notificationApplication = notificationApplication;
            _resetPasswordApplication = resetPasswordApplication;
        }

        public Task<List<UserViewModel>> Search(UserSearchModel searchModel)
        {
            return _userRepository.Search(searchModel);
        }
        public Task<List<RecipientViewModel>> GetUserListForListing(long id)
        {
            return _userRepository.GetUserListForListing(id);
        }
        public async Task<OperationResult> Register(RegisterUser command)
        {
            var result = new OperationResult();
            if (_userRepository.Exist(x => x.Email == command.Email))
                return result.Failed(ApplicationMessage.RecordExists);

            var RedirectUrl = _contextAccessor.HttpContext.Request.Headers
                .FirstOrDefault(x => x.Key == "Origin").Value
                .ToString();

            // Check the role for status management
            var roles = await _roleRepository.GetAll();
            command.Status = false;
            var check = roles.FirstOrDefault(x => x.Name == "Customer of Raw Material").Id;
            if (command.RoleId == check)
                command.Status = true;


            var password = _passwordHasher.Hash(command.Password);
            var activationGuid = Guid.NewGuid();
            var request = _contextAccessor.HttpContext.Request;

            var emailModel = new EmailModel
            {
                EmailTemplate = 1,
                Title = ApplicationMessage.AccountVerification,
                AccountVerificationLink = $"http://{request.Host}/Authentication/ActivateUser/{activationGuid.ToString()}".ToLower(),
                Recipient = command.Email
            };
            var emailServiceResult = _emailService.SendEmail(emailModel);

            if (emailServiceResult.IsSucceeded)
            {
                var user = new User(command.Email, password, activationGuid, command.RoleId, command.Status);
                _userRepository.Create(user);
                _userRepository.SaveChanges();

                /// Notification

                var systemNotificationId = _notificationApplication
                    .PushNotification(new NotificationViewModel
                    {
                        SenderId = 1,
                        RedirectUrl = RedirectUrl + $"/dashboard/profile/{user.Id}",
                        NotificationBody = ApplicationMessage.ListingNewItemListed,
                        NotificationTitle = ApplicationMessage.SystemMessage,
                        UserId = user.Id
                    });

                _recipientRepository.Create(new Recipient(user.Id, command.RoleId, systemNotificationId.Result));
                _recipientRepository.SaveChanges();

                return result.Succeeded();
            }
            else
            {
                return emailServiceResult;
            }
        }
        public async Task<OperationResult> ActivateUser(string command)
        {
            var request = _contextAccessor.HttpContext.Request;
            var result = new OperationResult();
            if (!string.IsNullOrWhiteSpace(command))
            {
                var user = await _userRepository.Get(_userRepository.GetDetailByActivationUrl(command).Id);
                if (user != null)
                {
                    if (user.RoleId != 5)
                    {
                        var emailModel = new EmailModel
                        {
                            EmailTemplate = 2,
                            Title = ApplicationMessage.AccountActivated,
                            Body = "Welcome to Circ4Bio",
                            Body1 = "Your account has successfully registered.",
                            Body2 = "Please provide your company name and the VAT number and, our team will verify " +
                                    "the provided information within 24 hours." +
                                    "It is just the formal background check of your company to protect other clients." +
                                    "Please follow the link below to update your profile",
                            Body3 = $"http://{request.Host}/Dashboard/Profile/{user.UserName}".ToLower(),
                            Recipient = user.Email
                        };
                        var emailServiceResult = _emailService.SendEmail(emailModel);

                        if (emailServiceResult.IsSucceeded)
                        {
                            user.ActivateUserStatus();
                            _userRepository.SaveChanges();
                        }
                        else
                        {
                            return emailServiceResult;
                        }
                    }
                    else
                    {
                        user.ActivateUserStatus();
                        user.ActivateUser();
                    }

                    return result.Succeeded();
                }
                return result.Failed(ApplicationMessage.ActivationUrlError);
            }
            return result.Failed(ApplicationMessage.SomethingWentWrong);

        }
        public async Task<OperationResult> AdminActivateUser(long Id)
        {
            var result = new OperationResult();

            var user = await _userRepository.Get(Id);
            if (user != null)
            {
                user.ActivateUser();
                _userRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);

        }
        public async Task<OperationResult> SendActivationEmail(string command)
        {
            var result = new OperationResult();
            if (!_userRepository.Exist(x => x.Email == command))
                return result.Failed(ApplicationMessage.RecordNotFound);

            var request = _contextAccessor.HttpContext.Request;
            var activationGuid = _userRepository.ResendActivationLink(command).Result.ActivationGuid;

            var emailModel = new EmailModel
            {
                EmailTemplate = 1,
                Title = ApplicationMessage.AccountVerification,
                AccountVerificationLink = $"http://{request.Host}/Authentication/ActivateUser/{activationGuid.ToString()}".ToLower(),
                Recipient = command
            };
            var emailServiceResult = _emailService.SendEmail(emailModel);

            if (emailServiceResult.IsSucceeded)
            {
                return result.Succeeded();
            }
            else
            {
                return emailServiceResult;
            }
        }
        public async Task<OperationResult> AdminDectivateUser(long Id)
        {
            var result = new OperationResult();

            var user = await _userRepository.Get(Id);
            if (user != null)
            {
                user.DeactivateUser();
                _userRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);
        }
        public async Task<OperationResult> AdminActivateUserStatus(long Id)
        {
            var result = new OperationResult();

            var user = await _userRepository.Get(Id);
            if (user != null)
            {
                user.ActivateUserStatus();
                _userRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);
        }
        public async Task<OperationResult> AdminDectivateUserStatus(long Id)
        {
            var result = new OperationResult();

            var user = await _userRepository.Get(Id);
            if (user != null)
            {
                user.DeactivateUserStatus();
                _userRepository.SaveChanges();
                return result.Succeeded();
            }

            return result.Failed(ApplicationMessage.RecordNotFound);
        }
        public async Task<OperationResult> EditByAdmin(EditUser command)
        {
            var result = new OperationResult();
            var user = await _userRepository.Get(command.Id);

            if (_userRepository.Exist(x => x.UserName == command.UserId && x.Id != command.Id))
                return result.Failed(ApplicationMessage.RecordExists);

            if (command.CompanyName != null)
            {
                if (_userRepository.Exist(x => x.CompanyName == command.CompanyName && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.VatNumber != 0)
            {
                if (_userRepository.Exist(x => x.VatNumber == command.VatNumber && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.FaceBookUrl != null)
            {
                if (_userRepository.Exist(x => x.FaceBookUrl == command.FaceBookUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.TwitterUrl != null)
            {
                if (_userRepository.Exist(x => x.TwitterUrl == command.TwitterUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.InstagramUrl != null)
            {
                if (_userRepository.Exist(x => x.InstagramUrl == command.InstagramUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.WebUrl != null)
            {
                if (_userRepository.Exist(x => x.WebUrl == command.WebUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }



            if (command.RoleId == 0)
                command.RoleId = int.Parse(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value);

            var avatarFileName = _fileUploader.Uploader(command.ProfilePicture, "Profile_Images", user.Id.ToString());

            user.Edit(command.FirstName, command.LastName, command.UserId, command.Email, command.City, command.Country,
                command.PostalCode, command.Latitude, command.Longitude, command.Description,
                command.CompanyName, command.VatNumber, avatarFileName, command.WebUrl, command.LinkdinUrl,
                command.TwitterUrl, command.InstagramUrl, command.FaceBookUrl, command.RoleId);

            _userRepository.SaveChanges();

            return result.Succeeded();
        }
        public async Task<OperationResult> EditByUser(EditUser command)
        {
            var result = new OperationResult();
            var user = await _userRepository.Get(command.Id);

            if (_userRepository.Exist(x => x.UserName == command.UserId && x.Id != command.Id))
                return result.Failed(ApplicationMessage.RecordExists);

            if (command.CompanyName != null)
            {
                if (_userRepository.Exist(x => x.CompanyName == command.CompanyName && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.VatNumber != 0)
            {
                if (_userRepository.Exist(x => x.VatNumber == command.VatNumber && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.FaceBookUrl != null)
            {
                if (_userRepository.Exist(x => x.FaceBookUrl == command.FaceBookUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.TwitterUrl != null)
            {
                if (_userRepository.Exist(x => x.TwitterUrl == command.TwitterUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.InstagramUrl != null)
            {
                if (_userRepository.Exist(x => x.InstagramUrl == command.InstagramUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            if (command.WebUrl != null)
            {
                if (_userRepository.Exist(x => x.WebUrl == command.WebUrl && x.Id != command.Id))
                    return result.Failed(ApplicationMessage.RecordExists);
            }

            var avatarFileName = _fileUploader.Uploader(command.ProfilePicture, "Profile_Images", user.Id.ToString());

            user.Edit(command.FirstName, command.LastName, command.UserId, command.Address, command.City, command.Country,
                command.PostalCode, command.Latitude, command.Longitude, command.Description,
                command.CompanyName, command.VatNumber, avatarFileName, command.WebUrl, command.LinkdinUrl,
                command.TwitterUrl, command.InstagramUrl, command.FaceBookUrl, user.RoleId);

            _userRepository.SaveChanges();

            return result.Succeeded();
        }
        public async Task<OperationResult> ChangePassword(ResetPasswordModel command)
        {
            var result = new OperationResult();
            var user = await _userRepository.Get(command.UserId);

            return result.Succeeded();
        }
        public Task<EditUser> GetDetail(long Id)
        {
            return _userRepository.GetDetail(Id);
        }
        public Task<EditUser> GetDetailByUsername(string username)
        {
            return _userRepository.GetDetailByUser(username);
        }
        public async Task<OperationResult> ResetPassword(ResetPasswordModel command)
        {
            var result = new OperationResult();
            var user = await _userRepository.Get(command.UserId);
            var password = _passwordHasher.Hash(command.Password);
            user.ChangePassword(password);
            _userRepository.SaveChanges();
            return result.Succeeded(ApplicationMessage.ResetPasswordSuccess);

        }
        public async Task<OperationResult> Login(EditUser command)
        {
            var result = new OperationResult();
            var user = await _userRepository.GetDetailByEmail(command.Email);

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
            if (user == null)
                return result.Failed(ApplicationMessage.UserNotExists);
            if (!user.IsActive)
                return result.Failed(ApplicationMessage.UserNotActive);


            var (verified, needsUpgrade) = _passwordHasher.Check(user.Password, command.Password);
            if (!verified)
                return result.Failed(ApplicationMessage.WrongPassword);

            var permissions = _roleRepository.GetDetail(user.RoleId).Result
                .MappedPermissions
                .Select(x => x.Code)
                .ToList();

            var authModel = new AuthViewModel(user.Id, user.Email, $"{user.FirstName} {user.LastName}",
                user.RoleId.ToString(), command.RememberMe, user.PictureString, permissions
                , command.Password, user.IsActive, user.Status);
            _authenticateHelper.Login(authModel);
            return result.Succeeded();
        }
        public void Logout()
        {
            _authenticateHelper.Logout();
        }
        public Task<List<Usertype>> GetUsertypes()
        {
            var type = new Usertype(0, "Default");
            return Task.FromResult(type.GetUserTypeList());
        }
        public async Task<OperationResult> AddDeliveryLocation(CreateDeliveryLocation Command)
        {
            var result = new OperationResult();
            if (await _userRepository.CheckDeliveryLocationName(Command.Name, Command.UserId))
            {
                if (_userRepository.Exist(x => x.Id == Command.UserId))
                {
                    return await _userRepository.AddDeliveryLocation(Command);
                }
                return result.Failed(ApplicationMessage.SomethingWentWrong);
            }
            else
            {
                return result.Failed(ApplicationMessage.DuplicateDeliveryLocationName);
            }
        }
        public async Task<bool> RemoveDeliveryLocation(CreateDeliveryLocation Command)
        {
            if (_userRepository.Exist(x => x.Id == Command.UserId))
            {
                return await _userRepository.RemoveDeliveryLocation(Command);
            }
            else
            {
                return false;
            }
        }
        public async Task<List<CreateDeliveryLocation>> GetDeliveryLocationList(long userId)
        {
            if (_userRepository.Exist(x => x.Id == userId))
            {
                return await _userRepository.GetDeliveryLocationList(userId);
            }
            else
            {
                return new List<CreateDeliveryLocation>();
            }

        }

        public Task<CreateDeliveryLocation> GetDeliveryLocation(long UserId, long LocationId)
        {
            if (_userRepository.Exist(x => x.Id == UserId))
            {
                return _userRepository.GetDeliveryLocation(UserId, LocationId);
            }
            else
            {
                return Task.FromResult(new CreateDeliveryLocation());
            }
        }


        public async Task<List<CreateDeliveryLocation>> GetDeliveryLocationDropDown(long userId)
        {
            if (_userRepository.Exist(x => x.Id == userId))
            {
                return await _userRepository.GetDeliveryLocationDropDown(userId);
            }
            else
            {
                return new List<CreateDeliveryLocation>();
            }
        }

        public async Task<OperationResult> EditDeliveryLocation(CreateDeliveryLocation Command)
        {
            var result = new OperationResult();
            if (_userRepository.Exist(x => x.Id == Command.UserId))
            {
                return await _userRepository.EditDeliveryLocation(Command);
            }

            return result.Failed(ApplicationMessage.SomethingWentWrong);
        }
    }
}
