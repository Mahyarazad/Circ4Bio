using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public Task<List<RecipientViewModel>> GetAllUnread(long Id)
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
                }).OrderByDescending(x => x.Id).ToListAsync();
        }

        public Task<OperationResult> MarkRead(long Id)
        {
            var result = new OperationResult();
            if (_amContext.Recipients.Any(x => x.Id == Id))
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            var target = _amContext.Recipients.FirstOrDefaultAsync(x => x.Id == Id);
            target.Result.MarkRead();
            _amContext.SaveChanges();
            return Task.FromResult(result.Succeeded());
        }
    }
}