using _0_Framework.Domain;
using AM.Domain.UserAggregate;

namespace AM.Domain.BlogAggregate
{
    public class Blog : BaseEntity<long>
    {
        public string? Title { get; private set; }
        public string? ShortDescription { get; private set; }
        public string? Body { get; private set; }
        public string? Image { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
    }
}