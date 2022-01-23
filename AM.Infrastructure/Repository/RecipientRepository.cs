using System.Collections.Generic;
using System.Linq;
using _0_Framework;
using _0_Framework.Application;
using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Notification;
using AM.Domain.NotificationAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class RecipientRepository : RepositoryBase<long, Recipient>, IRecipientRepository
    {
        private readonly AMContext _amContext;
        public RecipientRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<RecipientViewModel> GetAllUnread(long Id)
        {
            return _amContext.Recipients
                .AsNoTracking()
                .Where(x => !x.IsReed && x.UserId == Id)
                .Select(x => new RecipientViewModel
                {
                    Id = x.Id,
                    IsReed = x.IsReed,
                    RoleId = x.RoleId,
                    UserId = x.UserId,
                    NotificationId = x.NotificationId
                }).OrderByDescending(x => x.Id).ToList();
        }

        public OperationResult MarkRead(long Id)
        {
            var result = new OperationResult();
            if (_amContext.Recipients.Any(x => x.Id == Id))
                return result.Failed(ApplicationMessage.RecordNotFound);
            var target = _amContext.Recipients.FirstOrDefault(x => x.Id == Id);
            target.MarkRead();
            _amContext.SaveChanges();
            return result.Succeeded();
        }
    }
}