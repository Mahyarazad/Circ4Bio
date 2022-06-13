using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using AM.Domain.NaceAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class NaceRepository : RepositoryBase<long, Nace>, INaceRepository
    {
        private readonly AMContext _amContext;
        public NaceRepository(AMContext amContext) : base(amContext)
        {
            amContext = _amContext;
        }
    }
    public class NaceDataRepository : RepositoryBase<long, NaceData>, INaceDataRepository
    {
        private readonly AMContext _amContext;
        public NaceDataRepository(AMContext amContext) : base(amContext)
        {
            amContext = _amContext;
        }
    }
}
