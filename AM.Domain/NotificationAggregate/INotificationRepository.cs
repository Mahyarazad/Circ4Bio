﻿using System.Collections.Generic;
using _0_Framework.Domain;
using AM.Application.Contracts.Notification;

namespace AM.Domain.NotificationAggregate
{
    public interface INotificationRepository : IRepository<long, Notification>
    {
        List<RecipientViewModel> GetRecipientViewModel(long Id);
        List<NotificationViewModel> GetAllUnread(long Id);
        List<NotificationViewModel> GetLastNUnread(long Id, int nNumber);
        int CountUnRead(long Id);
    }
}