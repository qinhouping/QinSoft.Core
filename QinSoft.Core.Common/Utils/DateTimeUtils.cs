using System;
using System.Collections.Generic;
using System.Globalization;
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
            return (long)(dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }

        /// <summary>
        /// 获取时间
        /// </summary>
        public static DateTime ToDateTime(this long timeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(timeStamp).ToLocalTime();
        }

        /// <summary>
        /// 格式化时间格式
        /// </summary>
        public static string Format(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 解析时间
        /// </summary>
        public static DateTime Parse(string dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            DateTime dt = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
            DateTime.SpecifyKind(dt, DateTimeKind.Local);
            return dt;
        }
    }
}
