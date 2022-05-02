using System;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.User;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Notification;


namespace AM.Infrastructure.Repository
{
    public class UserRepository : RepositoryBase<long, User>, IUserRepository
    {
        private readonly AMContext _amContext;
        public UserRepository(AMContext amContext) : base(amContext)
        {

            _amContext = amContext;
        }
        public Task<List<UserViewModel>> Search(UserSearchModel searchModel)
        {
            var query = _amContext.Users
                .Include(x => x.Role)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Role = x.Role.Name,
                    IsActive = x.IsActive,
                    Avatar = x.Avatar,
                    Status = x.Status,
                    UserId = x.UserName,
                    FullName = $"{x.FirstName} {x.LastName}",
                    CreationTime = TruncateDateTime.TruncateToDefault(x.CreationTime).ToString()
                }).AsNoTracking();
            if (!string.IsNullOrEmpty(searchModel.Email))
                query = query.Where(x => x.Email.Contains(searchModel.Email));

            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
                query = query.Where(x => x.PhoneNumber.Contains(searchModel.PhoneNumber));
            if (!string.IsNullOrEmpty(searchModel.UserId))
                query = query.Where(x => x.UserId.Contains(searchModel.UserId));
            // if (searchModel.Role != null)
            //     query = query.Where(x => x.Role == searchModel.Role);
            return query.OrderByDescending(x => x.Id).ToListAsync();
        }

        public Task<List<UserViewModel>> GetFullList()
        {
            return _amContext.Users
                .Include(x => x.Role)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Role = x.Role.Name,
                    RoleId = x.Role.Id,
                    Status = x.Status,
                    UserId = x.UserName,
                }).Where(x => x.RoleId != 1)
                .AsNoTracking().ToListAsync();

        }
        public async Task<List<RecipientViewModel>> GetUserListForListing(long id)
        {
            return await _amContext.Users
                .Include(x => x.Role)
                .Where(x => x.Id != id && x.Id != 1)
                .Select(x => new RecipientViewModel
                {
                    UserId = x.Id,
                    RoleId = x.RoleId
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public Task<EditUser> GetDetail(long Id)
        {
            var query = _amContext.Users.Select(x => new EditUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserId = x.UserName,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PhoneNumber = x.PhoneNumber,
                RoleId = x.RoleId,
                Address = x.Address,
                City = x.City,
                Country = x.Country,
                CompanyName = x.CompanyName,
                Description = x.Description,
                FaceBookUrl = x.FaceBookUrl,
                InstagramUrl = x.InstagramUrl,
                PostalCode = x.PostalCode,
                LinkdinUrl = x.LinkdinUrl,
                VatNumber = x.VatNumber,
                TwitterUrl = x.TwitterUrl,
                Status = x.Status,
                Avatar = x.Avatar,

            }).AsNoTracking().FirstOrDefault(x => x.Id == Id);
            query.RoleString = _amContext.Roles.ToList().FirstOrDefault(x => x.Id == query.RoleId).Name;

            return Task.FromResult(query);

        }
        public Task<EditUser> GetDetailByUser(string username)
        {
            var query = _amContext.Users.Select(x => new EditUser
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                UserId = x.UserName,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PhoneNumber = x.PhoneNumber,
                RoleId = x.RoleId,
                Address = x.Address,
                City = x.City,
                Country = x.Country,
                CompanyName = x.CompanyName,
                Description = x.Description,
                FaceBookUrl = x.FaceBookUrl,
                InstagramUrl = x.InstagramUrl,
                PostalCode = x.PostalCode,
                LinkdinUrl = x.LinkdinUrl,
                VatNumber = x.VatNumber,
                TwitterUrl = x.TwitterUrl,
                Status = x.Status,
                Avatar = x.Avatar,

            }).AsNoTracking().FirstOrDefault(x => x.UserId == username);
            query.RoleString = _amContext.Roles.ToList().FirstOrDefault(x => x.Id == query.RoleId).Name;

            return Task.FromResult(query);
        }
        public Task<ResendActivationEmail> ResendActivationLink(string email)
        {
            return _amContext.Users.Select(x => new ResendActivationEmail
            {
                ActivationGuid = x.ActivationGuid,
                Email = x.Email
            }).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == email);
        }
        public Task<EditUser> GetDetailByEmail(string email)
        {
            return _amContext.Users.Select(x => new EditUser
            {
                Id = x.Id,
                Email = x.Email,
                Password = x.Password,
                RoleId = x.RoleId,
                Status = x.Status,
                IsActive = x.IsActive,
                FirstName = x.FirstName,
                LastName = x.LastName
            }).AsNoTracking().FirstOrDefaultAsync(x => x.Email == email);

        }
        public Task<EditUser> GetDetailByActivationUrl(string guid)
        {
            var query = _amContext.Users.Where(x => x.ActivationGuid == Guid.Parse(guid))
                .Select(x => new EditUser
                {
                    Id = x.Id,
                    ActivationGuid = x.ActivationGuid.ToString()

                }).AsNoTracking();

            if (query.First() != null)
            {
                return Task.FromResult(query.First());
            }
            else
            {
                return Task.FromResult(new EditUser());
            }
        }
        public async Task<OperationResult> AddDeliveryLocation(CreateDeliveryLocation Commad)
        {
            var result = new OperationResult();
            var deliveryLocationList = await _amContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == Commad.UserId);
            if (deliveryLocationList != null)
            {
                deliveryLocationList.AddDeliveryLocation(Commad.UserId, Commad.Name, Commad.AddressLineOne,
                    Commad.AddressLineTwo, Commad.City, Commad.Country, Commad.Latitude, Commad.Longitude, Commad.PostalCode);
                _amContext.SaveChanges();
                return result.Succeeded();
            }
            return result.Failed(ApplicationMessage.RecordExists);
        }

        public async Task<OperationResult> EditDeliveryLocation(CreateDeliveryLocation Commad)
        {
            var result = new OperationResult();
            var deliveryLocationList = await _amContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == Commad.UserId);
            if (deliveryLocationList != null)
            {
                deliveryLocationList.EditDeliveryLocation(Commad.UserId, Commad.Name, Commad.AddressLineOne,
                    Commad.AddressLineTwo, Commad.City, Commad.Country, Commad.Latitude
                    , Commad.Longitude, Commad.PostalCode, Commad.LocationId);
                _amContext.SaveChanges();
                return result.Succeeded();
            }
            return result.Failed(ApplicationMessage.RecordExists);
        }

        public async Task<bool> RemoveDeliveryLocation(CreateDeliveryLocation Commad)
        {
            var deliveryLocationList = await _amContext
                .Users
                .FirstOrDefaultAsync(x => x.Id == Commad.UserId);
            if (deliveryLocationList != null)
            {
                var result = deliveryLocationList.RemoveDeliveryLocation(Commad.LocationId);
                _amContext.SaveChanges();
                return result;
            }
            return false;
        }
        public Task<List<CreateDeliveryLocation>> GetDeliveryLocationList(long userId)
        {
            var query = _amContext.Users
                .Where(x => x.Id == userId)
                .AsNoTracking().AsSplitQuery().First().DeliveryLocations;
            var result = query.Select(x => new CreateDeliveryLocation
            {
                AddressLineOne = x.AddressLineOne,
                AddressLineTwo = x.AddressLineTwo,
                Name = x.Name,
                City = x.City,
                Country = x.Country,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PostalCode = x.PostalCode,
                LocationId = x.Id,
                UserId = x.UserId
            }).ToList();

            return Task.FromResult(result);

        }

        public Task<CreateDeliveryLocation> GetDeliveryLocation(long userId, long locationId)
        {
            var query = _amContext.Users
                .Where(x => x.Id == userId)
                .AsNoTracking().AsSplitQuery().FirstAsync().Result.DeliveryLocations;

            var result = query.Select(x => new CreateDeliveryLocation
            {
                Name = x.Name,
                LocationId = x.Id,
                AddressLineOne = x.AddressLineOne,
                AddressLineTwo = x.AddressLineTwo,
                City = x.City,
                Country = x.Country,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                PostalCode = x.PostalCode,
                UserId = x.UserId
            }).FirstOrDefault(x => x.LocationId == locationId);

            return Task.FromResult(result);
        }

        public async Task<List<CreateDeliveryLocation>> GetDeliveryLocationDropDown(long userId)
        {
            var query = _amContext.Users
                .Where(x => x.Id == userId)
                .AsNoTracking().AsSplitQuery().FirstAsync().Result.DeliveryLocations;
            var result = query.Select(x => new CreateDeliveryLocation
            {
                Name = x.Name,
                LocationId = x.Id
            }).ToList();
            return result;
        }

        public Task<bool> CheckDeliveryLocationName(string Name, long userId)
        {
            var query = _amContext.Users
                .Where(x => x.Id == userId)
                .AsNoTracking().AsSplitQuery().First().DeliveryLocations;

            return Task.FromResult(!query.Any(x => x.Name == Name));

        }
    }
}