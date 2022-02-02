using System;
using System.Collections.Generic;
using System.Linq;
using AM.Application.Contracts.Negotiate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Dashboard.Pages.Negotiate
{
    public class MessagesModel : PageModel
    {
        public NewMessage Command;
        public List<MessageViewModel> MessageList;
        public NegotiateViewModel CurrentNegotiate;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INegotiateApplication _negotiateApplication;

        public MessagesModel(INegotiateApplication negotiateApplication,
            IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            _negotiateApplication = negotiateApplication;
        }

        public void OnGet(long Id)
        {
            CurrentNegotiate = _negotiateApplication.GetNegotiationViewModel(Id);
            MessageList = new List<MessageViewModel>();
            MessageList = _negotiateApplication.GetMessages(Id);
        }

        public JsonResult OnPost(NewMessage Command)
        {
            Command.UserEntity = false;
            CurrentNegotiate = _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
            Command.UserId = Convert.ToInt64(
                _contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
            if (Command.UserId == CurrentNegotiate.BuyerId)
                Command.UserEntity = true;
            var res = _negotiateApplication.SendMessage(Command);
            return new JsonResult(res);

        }
    }
}
