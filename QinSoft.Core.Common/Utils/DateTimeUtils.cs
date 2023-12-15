using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 时间工具类
    /// </summary>
    public static class DateTimeUtils
    {
        public enum TimestampPrecision
        {
            S,
            Ms,
            Us,
            Ns
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        public static long ToTimestamp(this DateTime dateTime, TimestampPrecision precision = TimestampPrecision.Ms)
        {
            TimeSpan timeSpan = dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            switch (precision)
            {
                case TimestampPrecision.Ns: return timeSpan.Ticks * 100;
                case TimestampPrecision.Us: return (long)(timeSpan.Ticks * 0.1);
                case TimestampPrecision.Ms: return (long)timeSpan.TotalMilliseconds;
                case TimestampPrecision.S: return (long)timeSpan.TotalSeconds;
                default: throw new ArgumentException("precision");
            };
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
        public static string FormatDateTime(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            return dateTime.ToString(format);
        }

        /// <summary>
        /// 解析时间
        /// </summary>
        public static DateTime ParseDateTime(string dateTime, string format = "yyyy-MM-dd HH:mm:ss")
        {
            DateTime dt = DateTime.ParseExact(dateTime, format, CultureInfo.InvariantCulture);
            DateTime.SpecifyKind(dt, DateTimeKind.Local);
            return dt;
        }
    }
}
