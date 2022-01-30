namespace AM.Application.Contracts.Negotiate
{
    public class NewMessage
    {
        public long NegotiateId { get; set; }
        public long UserId { get; set; }
        public string? MessageBody { get; set; }
        public string? UserEntity { get; set; }
    }
}