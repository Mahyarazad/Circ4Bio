using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost.Areas.Dashboard.Pages.Negotiate
{
    public class MessagesModel : PageModel
    {
        public NewMessage Command;
        public List<MessageViewModel> MessageList;
        public NegotiateViewModel CurrentNegotiate;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;

        public MessagesModel(INegotiateApplication negotiateApplication,
            IAuthenticateHelper authenticateHelper)
        {
            _authenticateHelper = authenticateHelper;
            _negotiateApplication = negotiateApplication;
        }

        public async Task<IActionResult> OnGet(long Id)
        {
            var loggedInUserId = _authenticateHelper.CurrentAccountRole().Id;
            CurrentNegotiate = await _negotiateApplication.GetNegotiationViewModel(Id);
            if (CurrentNegotiate.SellerId == loggedInUserId ||
                CurrentNegotiate.BuyerId == loggedInUserId)
            {
                Command = new NewMessage();
                MessageList = new List<MessageViewModel>();
                MessageList = await _negotiateApplication.GetMessages(Id);
                return null;
            }
            else
            {
                return RedirectToPage("/AccessDenied", new { area = "" });
            }
        }

        public async Task<JsonResult> OnPost(NewMessage Command)
        {
            Command.UserEntity = false;
            CurrentNegotiate = await _negotiateApplication.GetNegotiationViewModel(Command.NegotiateId);
            Command.UserId = _authenticateHelper.CurrentAccountRole().Id;
            if (Command.UserId == CurrentNegotiate.BuyerId)
                Command.UserEntity = true;
            var res = _negotiateApplication.SendMessage(Command);
            return new JsonResult(res);
        }
    }
}
