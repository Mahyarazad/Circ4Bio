using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Negotiate;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace AM.Application
{
    [Authorize]
    public class NotificationHub : Hub
    {
        public NotificationHub(IHttpContextAccessor contextAccessor
            , IAuthenticateHelper authenticateHelper,
            INegotiateApplication negotiateApplication
            , INotificationApplication notificationApplication)
        {
            _contextAccessor = contextAccessor;
            _authenticateHelper = authenticateHelper;
            _negotiateApplication = negotiateApplication;
            _notificationApplication = notificationApplication;
        }

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthenticateHelper _authenticateHelper;
        private readonly INegotiateApplication _negotiateApplication;
        private readonly INotificationApplication _notificationApplication;
        public long UserId { get; set; }
        public async Task<int> CountUnreadNotification()
        {

            if (_contextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(_authenticateHelper.CurrentAccountRole().Id);
                return await _notificationApplication.CountUnread(UserId);
            }
            return 0;
        }

        public async Task<List<NotificationViewModel>> Notification()
        {
            if (_contextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                List<NotificationViewModel> result = await _notificationApplication.GetLastNUnread(UserId, 5);
                return result;

            }
            return new List<NotificationViewModel>();
        }
        public async Task<List<NotificationViewModel>> AllNotification()
        {
            if (_contextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                List<NotificationViewModel> result = await _notificationApplication.GetAllUnread(UserId);
                return result;

            }
            return new List<NotificationViewModel>();
        }

        public async Task<int> MarkRead(long notificationId)
        {

            if (_contextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                await _notificationApplication.MarkRead(notificationId);
                return _notificationApplication.CountUnread(UserId).Result;
            }

            return 0;

        }


        public async Task MarkReadAll()
        {
            if (_contextAccessor.HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(_contextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                await _notificationApplication.MarkAllRead(UserId);
            }
        }

        public async Task SendMessage(string messageBody, string negotiateId, string loggedUser)
        {
            var Command = new NewMessage();
            var CurrentNegotiate = new NegotiateViewModel();
            Command.UserEntity = false;
            Command.MessageBody = messageBody;
            Command.NegotiateId = Convert.ToInt64(negotiateId);
            // Command.File = fileInput;
            CurrentNegotiate = _negotiateApplication.GetNegotiationViewModel(Convert.ToInt64(negotiateId));
            Command.UserId = _authenticateHelper.CurrentAccountRole().Id;
            if (Command.UserId == CurrentNegotiate.BuyerId)
                Command.UserEntity = true;

            await _negotiateApplication.SendMessage(Command);

            await Clients.User(CurrentNegotiate.BuyerId.ToString())
                .SendAsync("ReceiveMessage", $"http://{_contextAccessor.HttpContext.Request.Host}"
                    , messageBody, negotiateId, CurrentNegotiate.SellerId.ToString(), Command.UserId.ToString()
                    , CurrentNegotiate.BuyerId.ToString()
                    , CurrentNegotiate.BuyerImageString, CurrentNegotiate.SellerImageString
                    , CurrentNegotiate.BuyerEmail.Substring(0, 1)
                    , CurrentNegotiate.SellerEmail.Substring(0, 1));

            await Clients.User(CurrentNegotiate.SellerId.ToString())
                .SendAsync("ReceiveMessage", $"http://{_contextAccessor.HttpContext.Request.Host}"
                    , messageBody, negotiateId, CurrentNegotiate.BuyerId.ToString(), Command.UserId.ToString()
                    , CurrentNegotiate.BuyerId.ToString()
                    , CurrentNegotiate.BuyerImageString
                    , CurrentNegotiate.SellerImageString
                    , CurrentNegotiate.BuyerEmail.Substring(0, 1), CurrentNegotiate.SellerEmail.Substring(0, 1));

            await Task.Run(CountUnreadNotification);

        }
        public List<MessageViewModel> GetAllMessages(long NegotiateId)
        {
            return _negotiateApplication.GetMessages(NegotiateId);
        }
    }
}