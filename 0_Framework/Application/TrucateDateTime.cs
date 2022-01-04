using System;

namespace _0_Framework.Application
{
    public static class TruncateDateTime
    {
        public static DateTime TruncateToDefault(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}