using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 对象工具类
    /// </summary>
    public static class ObjectUtils
    {
        /// <summary>
        /// 判断参数是否是NULL
        /// </summary>
        public static void CheckNull<T>(this T argument, string paramName)
        {
            if (argument == null)
            {
                throw new ArgumentException(paramName);
            }
        }

        /// <summary>
        /// 判断参数是否是NULL
        /// </summary>
        public static T CheckNull<T>(this T argument, Func<T> action)
        {
            if (argument == null)
            {
                return action();
            }
            return argument;
        }

        /// <summary>
        /// 判断参数是否超过范围
        /// </summary>
        public static void CheckRange<T>(this T argument, T begin, T end, string paramName) where T : IComparable
        {
            if (argument.CompareTo(begin) < 0 || argument.CompareTo(end) >= 0)
            {
                throw new ArgumentException(paramName);
            }
        }

        /// <summary>
        /// 判断参数是否超过范围
        /// </summary>
        public static T CheckRange<T>(this T argument, T begin, T end, Func<T> action) where T : IComparable
        {
            if (argument.CompareTo(begin) < 0 || argument.CompareTo(end) >= 0)
            {
                return action();
            }
            return argument;
        }

        /// <summary>
        /// 判断列表是否是空
        /// </summary>
        public static void CheckEmpty<T>(this IEnumerable<T> argument, string paramName)
        {
            if (argument.IsEmpty())
            {
                throw new ArgumentException(paramName);
            }
        }

        /// <summary>
        /// 判断列表是否是空
        /// </summary>
        public static T CheckEmpty<T, I>(this T argument, Func<T> action) where T : IEnumerable<I>
        {
            if (argument.IsEmpty())
            {
                return action();
            }
            return argument;
        }

        /// <summary>
        /// 通用检测
        /// </summary>
        public static T Check<T>(this T arguemnt, Func<T, T> checkAction)
        {
            return checkAction(arguemnt);
        }
    }
}
