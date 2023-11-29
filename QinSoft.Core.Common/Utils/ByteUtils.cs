using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace QinSoft.Core.Common.Utils
{
    public static class ByteUtils
    {
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
    }
}
