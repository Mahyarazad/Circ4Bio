using AM.Domain.UserAggregate;

namespace AM.Domain.NegotiateAggregate
{
    public class UserNegotiate
    {
        protected UserNegotiate()
        {

        }
        public UserNegotiate(long userId, long negotiateId, bool buyerBool)
        {
            UserId = userId;
            NegotiateId = negotiateId;
            BuyerBool = buyerBool;
        }

        public long UserId { get; private set; }
        public bool BuyerBool { get; private set; }
        public User? User { get; private set; }
        public long NegotiateId { get; private set; }
        public Negotiate? Negotiate { get; private set; }
    }
}