using System;
using System.Collections;
using System.Collections.Generic;
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
    }
}
