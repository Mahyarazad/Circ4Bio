﻿using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Notification;
using AM.Domain.UserAggregate;

namespace AM.Domain.NotificationAggregate
{
    public class Notification : BaseEntity<long>
    {
        protected Notification()
        {

        }

        public Notification(string? notificationBody, string? notificationTitle,
            long senderId, long userId)
        {
            NotificationBody = notificationBody;
            NotificationTitle = notificationTitle;
            isDeleted = false;
            SenderId = senderId;
            UserId = userId;
        }

        public string? NotificationBody { get; private set; }
        public string? NotificationTitle { get; private set; }
        public long SenderId { get; private set; }
        public bool isDeleted { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        public List<Recipient>? Recipient { get; private set; }
    }
}
