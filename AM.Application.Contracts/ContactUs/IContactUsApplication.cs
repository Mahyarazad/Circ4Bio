using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.ContactUs
{
    public interface IContactUsApplication
    {
        OperationResult CreateMessage(CreateMessage command);
        OperationResult MarkAsRead(long Id);

        List<ContactUsViewModel> GetContactUsMessages();
        List<ContactUsViewModel> GetAllContactUsMessages();
        List<ContactUsViewModel> GetReadContactUsMessages();
        ContactUsViewModel GetSingleMessages(long Id);
    }
}