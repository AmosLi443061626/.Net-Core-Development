﻿using System;

namespace CoreCommon.LinkZipkin
{
    public static class TimeUtils
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Create a UNIX timestamp from a UTC date time. Time is expressed in microseconds and not seconds.
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Unix_time"/>
        /// <param name="utcDateTime"></param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime utcDateTime)
        {
            return (long)(utcDateTime.ToUniversalTime().Subtract(Epoch).TotalMilliseconds * 1000L);
        }

        public static long ToTimestamp(this DateTime utcDateTime)
        {
            return (long)(utcDateTime.ToUniversalTime().Subtract(Epoch).TotalMilliseconds);
        }
    }
}
