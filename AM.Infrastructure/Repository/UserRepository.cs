﻿using System.Collections.Generic;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.User;
using AM.Domain.UserAggregate;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using _0_Framework.Application;

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
                    CreationTime = TruncateDateTime.TruncateToDefault(x.CreationTime).ToString()
                });
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

        public EditUser GetDetail(long Id)
        {
            return _amContext.Users.Select(x => new EditUser
            {
                Id = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                RoleId = x.RoleId,
            }).FirstOrDefault(x => x.Id == Id);
        }

        public EditUser GetDetailByUser(string username)
        {
            return _amContext.Users.Select(x => new EditUser
            {
                Id = x.Id,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                RoleId = x.RoleId,
                Password = x.Password,
            }).FirstOrDefault(x => x.UserId == username);
        }

        public ChangePassword getDetailforChangePassword(long Id)
        {
            return _amContext.Users.Select(x => new ChangePassword
            {
                Email = x.Email,
                Id = x.Id
            }).FirstOrDefault(x => x.Id == Id);
        }
    }
}