using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
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
        private readonly IAutenticateHelper _autenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;

        public MessagesModel(INegotiateApplication negotiateApplication,
            IAutenticateHelper autenticateHelper)
        {
            _autenticateHelper = autenticateHelper;
            _negotiateApplication = negotiateApplication;
        }

        public IActionResult OnGet(long Id)
        {
            var loggedInUserId = _autenticateHelper.CurrentAccountRole().Id;
            CurrentNegotiate = _negotiateApplication.GetNegotiationViewModel(Id);
            if (CurrentNegotiate.SellerId == loggedInUserId ||
                CurrentNegotiate.BuyerId == loggedInUserId)
            {
                MessageList = new List<MessageViewModel>();
                MessageList = _negotiateApplication.GetMessages(Id);
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public JsonResult OnPost(NewMessage Command)
        {
            Command.UserEntity = false;
            CurrentNegotiate = _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
            Command.UserId = _autenticateHelper.CurrentAccountRole().Id;
            if (Command.UserId == CurrentNegotiate.BuyerId)
                Command.UserEntity = true;
            var res = _negotiateApplication.SendMessage(Command);
            return new JsonResult(res);
        }
    }
}
