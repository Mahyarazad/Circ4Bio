using _0_Framework.Domain;

namespace AM.Domain.ContactUsAggregate
{
    public class ContactUs : BaseEntity<long>
    {
        protected ContactUs()
        {

        }
        public ContactUs(string? fullName, string? email, string? body, string? subject, string? phone)
        {
            FullName = fullName;
            Email = email;
            Subject = subject;
            Phone = phone;
            Body = body;
            IsRead = false;
        }

        public string? FullName { get; private set; }
        public string? Email { get; private set; }
        public string? Phone { get; private set; }
        public string? Subject { get; private set; }
        public string? Body { get; private set; }
        public bool IsRead { get; private set; }
    }
}