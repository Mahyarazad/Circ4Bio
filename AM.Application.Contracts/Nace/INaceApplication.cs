using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Nace
{
    public interface INaceApplication
    {
        Task<OperationResult> CreateNace(CreateNace Command);
        Task<OperationResult> EditNace(EditNace Command);
        Task<OperationResult> DeleteNace(long Id);
        Task<OperationResult> UndeleteNace(long Id);
        Task<List<NaceViewModel>> GetAllNaces();
        Task<NaceViewModel> GetSingleNace(long Id);
        Task<EditNace> EditSingleNace(long Id);
    }
}