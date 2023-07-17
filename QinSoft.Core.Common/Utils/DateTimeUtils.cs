using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public static class DateTimeUtils
    {
        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static long ToTimeStamp(this DateTime dateTime)
        {
            TimeSpan timeSpan = TimeZoneInfo.ConvertTimeToUtc(dateTime) - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)timeSpan.TotalMilliseconds;
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        public static DateTime ToDateTime(this long timeStamp)
        {
            DateTime date = new DateTime(1970, 1, 1, 8, 0, 0, DateTimeKind.Local);
            return date.AddMilliseconds(timeStamp);
        }
    }
}
