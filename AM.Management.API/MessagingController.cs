using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application;
using AM.Application.Contracts.Negotiate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagingController : ControllerBase
    {
        private readonly INegotiateApplication _negotiateApplication;
        private readonly IHubContext<ChatHub> _hubContext;
        public long UserId { get; set; }

        public MessagingController(INegotiateApplication negotiateApplication, IHubContext<ChatHub> hubContext)
        {
            _negotiateApplication = negotiateApplication;
            _hubContext = hubContext;
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

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> LoadMessages()
        {
            await _hubContext.Clients.All.SendAsync("loadmessages", _negotiateApplication.GetMessages(5));
            return Ok();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> AddChatMessage(MessageViewModel message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);

            // _hubContext.Groups.AddToGroupAsync(model)
            return new NoContentResult();

        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> PostFile(IFormFile fileForm)
        {

            // _hubContext.Groups.AddToGroupAsync(model)
            return new NoContentResult();

        }
    }

}