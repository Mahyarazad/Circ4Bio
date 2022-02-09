using System;
using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.Notification;
using AM.Application.Contracts.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Internal;
using Nancy.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDeliveryAddressController : ControllerBase
    {
        private readonly IUserApplication _userApplication;
        private readonly IAutenticateHelper _autenticateHelper;

        public UserDeliveryAddressController(IUserApplication userApplication,
            IAutenticateHelper autenticateHelper)
        {
            _userApplication = userApplication;
            _autenticateHelper = autenticateHelper;
        }

        [Route("[action]")]
        [HttpPost]
        public bool RemoveDeliveryLocation(dynamic Command)
        {
            if (_autenticateHelper.IsAuthenticated())
            {
                JObject jsonObject = JObject.Parse(Command.ToString());
                var locationId = Convert.ToInt32(jsonObject.First.First);
                var target = new CreateDeliveryLocation
                {
                    UserId = _autenticateHelper.CurrentAccountRole().Id,
                    LocationId = locationId
                };
                return _userApplication.RemoveDeliveryLocation(target);
            }

            return false;
        }
    }

}
