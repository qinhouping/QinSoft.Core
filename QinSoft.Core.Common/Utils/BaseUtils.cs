using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 基本工具类
    /// </summary>
    public static class BaseUtils
    {
        /// <summary>
        /// 判断列表是否是空
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> list)
        {
            return list == null || !list.Any();
        }

        /// <summary>
        /// 判断列表是否不是空
        /// </summary>
        public static bool IsNotEmpty<T>(this IEnumerable<T> list)
        {
            return list != null && list.Any();
        }

        /// <summary>
        /// 解析枚举
        /// </summary>
        public static TEnum? ParseEnum<TEnum>(this string name) where TEnum : struct
        {
            if (Enum.TryParse<TEnum>(name, out TEnum res))
            {
                return res;
            }
            return null;
        }

        public static string ToString(this byte[] bytes, Encoding encoding)
        {
            if (bytes == null) return null;
            return encoding.GetString(bytes);
        }

        public static string ToString(this byte[] bytes)
        {
            return ToString(bytes, Encoding.Default);
        }

        public static byte[] ToBytes(this string str, Encoding encoding)
        {
            if (str == null) return null;
            return encoding.GetBytes(str);
        }

        public static byte[] ToBytes(this string str)
        {
            return ToBytes(str, Encoding.Default);
        }

        public static Stream ToStream(this byte[] bytes)
        {
            MemoryStream ms = new MemoryStream(bytes);
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        public static byte[] ToBytes(this Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
