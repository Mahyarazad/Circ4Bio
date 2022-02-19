using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Negotiate;
using Microsoft.AspNetCore.Mvc;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly INegotiateApplication _negotiateApplication;
        public long UserId { get; set; }
        public MessagingController(INegotiateApplication negotiateApplication)
        {
            _negotiateApplication = negotiateApplication;
        }

        //NegotiateId
        [Route("[action]")]
        [HttpPost]
        public async Task<List<MessageViewModel>> GetMessages(CreateNegotiate Command)
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                var securityControl = await _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
                if (securityControl.BuyerId == UserId || securityControl.SellerId == UserId)
                {
                    return _negotiateApplication.GetMessages(Command.NegotiateId).Result;
                }
                else
                {
                    new List<MessageViewModel>();
                }
            }

            return new List<MessageViewModel>();
        }
    }

}