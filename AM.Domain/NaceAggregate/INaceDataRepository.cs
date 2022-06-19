using _0_Framework.Domain;
using AM.Application.Contracts.Nace;

namespace AM.Domain.NaceAggregate
{
    public interface INaceDataRepository : IRepository<long, NaceData>
    {
        NaceDataViewModel GetNaceData(long ListingId);
    }
}