using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace AM.Application
{
    // [HubName("chathub")]
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;

        public ChatHub(INegotiateApplication negotiateApplication,
            IAuthenticateHelper authenticateHelper,
            IHttpContextAccessor contextAccessor
            )
        {
            _negotiateApplication = negotiateApplication;
            _authenticateHelper = authenticateHelper;
            _contextAccessor = contextAccessor;
        }

        public async Task SendMessage(string messageBody, string negotiateId, string loggedUser)
        {
            var Command = new NewMessage();
            var CurrentNegotiate = new NegotiateViewModel();
            Command.UserEntity = false;
            Command.MessageBody = messageBody;
            Command.NegotiateId = Convert.ToInt64(negotiateId);
            // Command.File = fileInput;
            CurrentNegotiate = await _negotiateApplication.GetNegotiationViewModel(Convert.ToInt64(negotiateId));
            Command.UserId = _authenticateHelper.CurrentAccountRole().Id;
            if (Command.UserId == CurrentNegotiate.BuyerId)
                Command.UserEntity = true;
            await _negotiateApplication.SendMessage(Command);


            await Clients.User(CurrentNegotiate.BuyerId.ToString())
                .SendAsync("ReceiveMessage", $"https://{_contextAccessor.HttpContext.Request.Host}"
                    , messageBody, negotiateId, CurrentNegotiate.SellerId.ToString(), Command.UserId.ToString()
                    , CurrentNegotiate.BuyerId.ToString()
                    , CurrentNegotiate.BuyerImageString, CurrentNegotiate.SellerImageString
                    , CurrentNegotiate.BuyerName.Substring(0, 1), CurrentNegotiate.SellerName.Substring(0, 1));

            await Clients.User(CurrentNegotiate.SellerId.ToString())
                .SendAsync("ReceiveMessage", $"https://{_contextAccessor.HttpContext.Request.Host}"
                    , messageBody, negotiateId, CurrentNegotiate.BuyerId.ToString(), Command.UserId.ToString()
                    , CurrentNegotiate.BuyerId.ToString()
                    , CurrentNegotiate.BuyerImageString
                    , CurrentNegotiate.SellerImageString
                    , CurrentNegotiate.BuyerName.Substring(0, 1), CurrentNegotiate.SellerName.Substring(0, 1));
        }
        public List<MessageViewModel> GetAllMessages(long NegotiateId)
        {
            return _negotiateApplication.GetMessages(NegotiateId).Result;
        }
    }
}
