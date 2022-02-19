using System;
using System.Linq;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDeliveryAddressController : ControllerBase
    {
        private readonly IUserApplication _userApplication;
        public long UserId { get; set; }
        public UserDeliveryAddressController(IUserApplication userApplication)
        {
            _userApplication = userApplication;
        }

        [Route("[action]")]
        [HttpPost]
        public bool RemoveDeliveryLocation(dynamic Command)
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                JObject jsonObject = JObject.Parse(Command.ToString());
                var locationId = Convert.ToInt32(jsonObject.First.First);
                var target = new CreateDeliveryLocation
                {
                    UserId = UserId,
                    LocationId = locationId
                };
                return _userApplication.RemoveDeliveryLocation(target);
            }

            return false;
        }

    }
}

