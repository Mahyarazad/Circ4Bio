using System;

namespace AM.Application.Contracts.Negotiate
{
    public class NewMessage
    {
        public long NegotiateId { get; set; }
        public long UserId { get; set; }
        public string? MessageBody { get; set; }
        public bool UserEntity { get; set; }
    }

    public class MessageViewModel : NewMessage
    {
        public long MessageId { get; set; }
        public string? BuyyerLetter { get; set; }
        public string? SellerLetter { get; set; }
        public string? BuyyerImageString { get; set; }
        public string? SellerImageString { get; set; }
        public DateTime CreationTime { get; set; }
    }
}