using System;

namespace _0_Framework.Domain
{
    public class BaseEntity<T>
    {
        public BaseEntity()
        {
            CreationTime = DateTime.Now;
        }

        public T? Id { get; set; }
        public DateTime CreationTime { get; set; }
    }
}