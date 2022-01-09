using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Nancy.Json;
using Newtonsoft.Json;

namespace _0_Framework.Application
{
    public class AuthenticateHelper : IAutenticateHelper
    {
        public AuthenticateHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private readonly IHttpContextAccessor _httpContextAccessor;
        public void Login(AuthViewModel model)
        {
            var permissions = JsonConvert.SerializeObject(model.Permissions);
            var claims = new List<Claim> {
                new Claim("User Id", model.Id.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim(ClaimTypes.Role, model.RoleId.ToString()),
                new Claim("Permissions", permissions)
            };


            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(2)

            };
            _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public void Logout()
        {
            _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public string Username()
        {
            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            return
                claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
        }
        public AuthViewModel CurrentAccountRole()
        {
            if (!IsAuthenticated())
                return new AuthViewModel();
            var claims = _httpContextAccessor.HttpContext.User.Claims.ToList();
            var result = new AuthViewModel();
            // result.Id = long.Parse(claims.FirstOrDefault(x => x.Type == "Account Id")?.Value);
            // result.Username = claims.FirstOrDefault(x => x.Type == "Username")?.Value;
            // result.Fullname = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            result.RoleId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            result.Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            // result.ProfilePicture = claims.FirstOrDefault(x => x.Type == "ProfilePicture")?.Value;
            return result;
        }

        public List<int> GetPermission()
        {
            var permissions = _httpContextAccessor
                .HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Permissions").Value;
            return JsonConvert.DeserializeObject<List<int>>(permissions);
        }
    }
}