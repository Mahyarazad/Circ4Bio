﻿using System;
using System.Collections.Generic;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.User;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Runtime.InteropServices;
using _0_Framework.Application;
using AM.Application.Contracts.Listing;
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
        public List<UserViewModel> Search(UserSearchModel searchModel)
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
            return query.OrderByDescending(x => x.Id).ToList();
        }
        public List<RecipientViewModel> GetUserListForListing(long id)
        {
            return _amContext.Users
                .Include(x => x.Role)
                .Where(x => x.Id != id && x.Id != 1)
                .Select(x => new RecipientViewModel
                {
                    UserId = x.Id,
                    RoleId = x.RoleId
                })
                .AsNoTracking()
                .ToList();
        }
        public EditUser GetDetail(long Id)
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

            return query;
        }
        public EditUser GetDetailByUser(string username)
        {
            return _amContext.Users.Select(x => new EditUser
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
        }
        public ResendActivationEmail ResendActivationLink(string email)
        {
            return _amContext.Users.Select(x => new ResendActivationEmail
            {
                ActivationGuid = x.ActivationGuid,
                Email = x.Email
            }).AsNoTracking()
                .FirstOrDefault(x => x.Email == email);
        }
        public EditUser GetDetailByEmail(string email)
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
            }).AsNoTracking().FirstOrDefault(x => x.Email == email);

        }
        public EditUser GetDetailByActivationUrl(string guid)
        {
            var query = _amContext.Users.Select(x => new EditUser
            {
                Id = x.Id,
                ActivationGuid = x.ActivationGuid.ToString()
            }).AsNoTracking();
            if (query.FirstOrDefault(x => x.ActivationGuid == guid) != null)
            {
                return query.FirstOrDefault(x => x.ActivationGuid == guid);
            }
            else
            {
                return new EditUser();
            }
        }
        public void AddDeliveryLocation(CreateDeliveryLocation Commad)
        {
            var deliveryLocationList = _amContext
                .Users
                .FirstOrDefault(x => x.Id == Commad.UserId);
            if (deliveryLocationList != null)
            {
                deliveryLocationList.AddDeliveryLocation(Commad.UserId, Commad.Location);
                _amContext.SaveChanges();
            }
            return;
        }
        public bool RemoveDeliveryLocation(CreateDeliveryLocation Commad)
        {
            var deliveryLocationList = _amContext
                .Users
                .FirstOrDefault(x => x.Id == Commad.UserId);
            if (deliveryLocationList != null)
            {
                var result = deliveryLocationList.RemoveDeliveryLocation(Commad.LocationId);
                _amContext.SaveChanges();
                return result;
            }
            return false;
        }
        public List<CreateDeliveryLocation> GetDeliveryLocationList(long userId)
        {
            var query = _amContext.Users
                .Where(x => x.Id == userId)
                .AsNoTracking().AsSplitQuery().First().DeliveryLocations;
            return query.Select(x => new CreateDeliveryLocation
            {
                Location = x.Location,
                LocationId = x.Id,
                UserId = x.UserId
            }).ToList();

        }
    }
}