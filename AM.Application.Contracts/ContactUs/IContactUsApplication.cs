using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.ContactUs
{
    public interface IContactUsApplication
    {
        Task<OperationResult> CreateMessage(CreateMessage command);
        Task<OperationResult> MarkAsRead(long Id);
        Task<List<ContactUsViewModel>> GetContactUsMessages();
        Task<List<ContactUsViewModel>> GetAllContactUsMessages();
        Task<List<ContactUsViewModel>> GetReadContactUsMessages();
        Task<ContactUsViewModel> GetSingleMessages(long Id);
    }
}