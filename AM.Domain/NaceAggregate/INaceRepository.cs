using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Domain;
using AM.Application.Contracts.Nace;

namespace AM.Domain.NaceAggregate
{
    public interface INaceRepository : IRepository<long, Nace>
    {
        List<NaceViewModel> GetAllNaces();
        NaceViewModel GetSingleNace(long Id);
        EditNace EditSingleNace(long Id);
    }
    public interface INaceDataRepository : IRepository<long, NaceData>
    {

    }
}