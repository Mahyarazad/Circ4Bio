using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;

        public MessagingController(IAuthenticateHelper authenticateHelper, INegotiateApplication negotiateApplication)
        {
            _authenticateHelper = authenticateHelper;
            _negotiateApplication = negotiateApplication;
        }

        public long UserId { get; set; }


        //NegotiateId
        [Route("[action]")]
        [HttpPost]
        public async Task<List<MessageViewModel>> GetMessages(CreateNegotiate Command)
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                var securityControl = _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
                if (securityControl.BuyerId == UserId || securityControl.SellerId == UserId)
                {
                    return _negotiateApplication.GetMessages(Command.NegotiateId);
                }
                else
                {
                    new List<MessageViewModel>();
                }
            }

            return new List<MessageViewModel>();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> LoadMessages()
        {
            // await _hubContext.Clients.All.SendAsync("loadmessages", _negotiateApplication.GetMessages(5));
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddChatMessage(MessageViewModel message)
        {
            // await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

            // _hubContext.Groups.AddToGroupAsync(model)
            return new NoContentResult();

        }


        [Route("[action]")]
        [HttpPost]
        public Task<OperationResult> FileUpload(IFormFile file)
        {

            var UserId = _authenticateHelper.CurrentAccountRole().Id;
            var CurrentNegotiate = _negotiateApplication.GetNegotiationViewModel(Convert.ToInt64(file.FileName));
            dynamic FileName = HttpContext.Request.Headers.Values;
            var res = _negotiateApplication.SendMessage(new NewMessage
            {
                NegotiateId = Convert.ToInt64(file.FileName),
                UserEntity = UserId == CurrentNegotiate.BuyerId ? true : false,
                File = file,
                UserId = UserId,
                MessageBody = "Document -->"

            });

            return Task.FromResult(new OperationResult().Failed(ApplicationMessage.SomethingWentWrong));
        }
    }

}