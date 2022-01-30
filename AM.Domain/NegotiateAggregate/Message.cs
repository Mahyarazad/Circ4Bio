using System;

namespace AM.Domain.NegotiateAggregate
{
    public class Message
    {
        protected Message() { }
        public Message(string? messageBody, long userId, string? userEntity)
        {
            MessageBody = messageBody;
            UserId = userId;
            UserEntity = userEntity;
            CreationTime = DateTime.Now;
        }

        public int Id { get; private set; }
        public string? MessageBody { get; private set; }
        public string? UserEntity { get; private set; }
        public DateTime CreationTime { get; private set; }
        public long NegotiateId { get; private set; }
        public Negotiate? Negotiate { get; private set; }
        public long UserId { get; private set; }
    }
}