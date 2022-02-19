using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AM.Application.Contracts.Notification;
using Microsoft.AspNetCore.Mvc;

namespace AM.Management.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationApplication _notificationApplication;
        public long UserId { get; set; }
        public NotificationController(INotificationApplication notificationApplication)
        {
            _notificationApplication = notificationApplication;
        }

        [HttpPost]
        public async Task<int> MarkRead(NotificationViewModel command)
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                await _notificationApplication.MarkRead(command.Id);
                return _notificationApplication.CountUnread(UserId).Result;
            }

            return 0;

        }
        [Route("[action]")]
        [HttpGet]
        public async Task<int> CountUnreadNotification()
        {

            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                return await _notificationApplication.CountUnread(UserId);
            }
            return 0;
        }
        [Route("[action]")]
        [HttpGet]
        public async Task<List<NotificationViewModel>> Notification()
        {
            if (HttpContext.User.Claims.FirstOrDefault() != null)
            {
                UserId = Convert.ToInt64(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "User Id").Value);
                List<NotificationViewModel> result = await _notificationApplication.GetLastNUnread(UserId, 5);
                return result;

            }

            return new List<NotificationViewModel>();
        }
    }

}
