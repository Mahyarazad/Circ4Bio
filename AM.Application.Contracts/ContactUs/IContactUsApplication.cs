using System.Collections.Generic;
using _0_Framework.Application;

namespace AM.Application.Contracts.ContactUs
{
    public interface IContactUsApplication
    {
        OperationResult CreateMessage(CreateMessage command);

        List<ContactUsViewModel> GetContactUsMessages();
    }
}