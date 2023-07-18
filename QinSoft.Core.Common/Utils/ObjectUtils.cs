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
        public static void CheckNull<T>(T argument, string paramName) where T : class
        {
            if (argument == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 判断参数是否是NULL
        /// </summary>
        public static void CheckNull<T>(T argument, Func<Exception> action) where T : class
        {
            if (argument == null)
            {
                throw action();
            }
        }

        /// <summary>
        /// 判断列表是否是空
        /// </summary>
        public static void CheckEmpty<T>(IEnumerable<T> argument, string paramName) where T : class
        {
            if (argument.IsEmpty())
            {
                throw new ArgumentNullException(paramName);
            }
        }

        /// <summary>
        /// 判断列表是否是空
        /// </summary>
        public static void CheckEmpty<T>(IEnumerable<T> argument, Func<Exception> action) where T : class
        {
            if (argument.IsEmpty())
            {
                throw action();
            }
        }
    }
}
