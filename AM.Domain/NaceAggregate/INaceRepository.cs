using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Domain;
using AM.Application.Contracts.Nace;

namespace AM.Domain.NaceAggregate
{
    public interface INaceRepository : IRepository<long, Nace>
    {
        List<NaceViewModel> GetAllNaces();
        NaceViewModel GetSingleNace(long iId);
        EditNace EditSingleNace(long id);
        void DeleteIndexDetail(long naceId, int indexId, int indexDetailId);
        void EditIndexDetail(long naceId, int indexId, int indexDetailId, string indexDetailString);
        void AddIndexDetail(long naceId, int indexDetailId, string indexDetailString);
        void AddIndex(long naceId, string indexString);
        int LastInputId(long naceId);
    }

}