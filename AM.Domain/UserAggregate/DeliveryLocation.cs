namespace AM.Domain.UserAggregate
{
    public class DeliveryLocation
    {
        protected DeliveryLocation() { }
        public DeliveryLocation(long userId, string? location)
        {
            UserId = userId;
            Location = location;
        }

        public int Id { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        public string? Location { get; private set; }
    }
}