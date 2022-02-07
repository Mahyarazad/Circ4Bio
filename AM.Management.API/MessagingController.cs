using System;
using System.Collections.Generic;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Mvc;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;

        public MessagingController(INegotiateApplication negotiateApplication,
            IAutenticateHelper autenticateHelper)
        {
            _autenticateHelper = autenticateHelper;
            _negotiateApplication = negotiateApplication;
        }
        //NegotiateId
        [Route("[action]")]
        [HttpPost]
        public List<MessageViewModel> GetMessages(CreateNegotiate Command)
        {
            var loggedUser = _autenticateHelper.CurrentAccountRole().Id;
            var securityControl = _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
            if (securityControl.BuyerId == loggedUser || securityControl.SellerId == loggedUser)
            {
                return _negotiateApplication.GetMessages(Command.NegotiateId);
            }
            else
            {
                return null;
            }
        }
    }

}
