using System.Threading.Tasks;
using _0_Framework.Application;
using AM.Application.Contracts.Nace;

namespace AM.Application.Contracts.NaceData
{
    public interface INaceDataApplication
    {
        Task<OperationResult> CreateNaceData(NaceDataDTO Command);
        Task<OperationResult> EditNaceData(NaceDataDTO Command);
        NaceDataViewModel GetNaceData(long ListingId);
        Task<OperationResult> DeleteNaceData(long Id);
    }
}