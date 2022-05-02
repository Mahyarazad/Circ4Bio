using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace _0_Framework.Application
{
    public class AuthenticateHelper : IAuthenticateHelper
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
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim("Permissions", permissions),
                new Claim("FullName", model.Fullname),
                new Claim("IsActive", model.IsActive.ToString()),
                new Claim("Status", model.Status.ToString()),
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
            result.Id = Convert.ToInt64(claims.FirstOrDefault(x => x.Type == "User Id").Value);
            result.Fullname = claims.FirstOrDefault(x => x.Type == "FullName").Value;
            result.RoleId = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            result.Email = claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            var splited = result.Email.Split("@");
            result.UserId = splited[0];
            result.IsActive = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "IsActive")?.Value);
            result.Status = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "Status")?.Value);
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