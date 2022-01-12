using _0_Framework.Infrastructure;
using AM.Domain.ContactUsAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class ContactUsRepository : RepositoryBase<long, ContactUs>, IContactUsRepository
    {
        private readonly AMContext _amContext;
        public ContactUsRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }
    }
}