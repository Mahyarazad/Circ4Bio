using System;
using System.IO;

namespace AM.Domain.NegotiateAggregate
{
    public class Message
    {
        protected Message() { }
        public Message(string? messageBody, long userId,
            bool userEntity, string? filePathString)
        {
            MessageBody = messageBody;
            UserId = userId;
            UserEntity = userEntity;
            CreationTime = DateTime.Now;
            IsRead = false;
            if (!string.IsNullOrWhiteSpace(filePathString))
                FilePathString = filePathString;
        }

        public long Id { get; private set; }
        public string? MessageBody { get; private set; }
        public string? FilePathString { get; private set; }
        //1 for buyyer 0 for seller
        public bool UserEntity { get; private set; }
        public bool IsRead { get; private set; }
        public DateTime CreationTime { get; private set; }
        public long NegotiateId { get; private set; }
        public Negotiate? Negotiate { get; private set; }
        public long UserId { get; private set; }
    }
}