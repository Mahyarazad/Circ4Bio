using System.Collections.Generic;
using System.Linq;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Nace;
using AM.Domain.NaceAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class NaceDataRepository : RepositoryBase<long, NaceData>, INaceDataRepository
    {
        private readonly AMContext _amContext;
        public NaceDataRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public NaceDataViewModel GetNaceData(long ListingId)
        {
            return _amContext.NaceDatas.AsSingleQuery()
                .Include(x => x.NaceDetailDatas)
                .Where(x => x.ListingId == ListingId)
                .Select(x => new NaceDataViewModel
                {
                    ListingId = x.ListingId,
                    NaceId = x.NaceId,
                    NaceDataDetails = x.NaceDetailDatas
                        .Select(y =>
                            new NaceDataDetail(y.ItemId, y.NaceData)).ToList()
                })
                    .First();
        }
    }
}