using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Domain;

namespace AM.Domain.UserAggregate
{
    public class ResetPassword
    {
        public ResetPassword(long userId, Guid resetUrl)
        {
            ResetUrl = resetUrl;
            UserId = userId;
            CreationTime = DateTime.Now;
            ExprateDateTime = DateTime.Now.AddMinutes(10);
            IsValid = ExprateDateTime > CreationTime;
        }

        public long Id { get; private set; }
        public long UserId { get; private set; }
        public Guid ResetUrl { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime ExprateDateTime { get; private set; }
        public bool IsValid { get; private set; }

    }
}
