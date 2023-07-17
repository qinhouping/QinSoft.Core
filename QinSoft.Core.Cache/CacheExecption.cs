using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache
{
    /// <summary>
    /// 缓存异常
    /// </summary>
    public class CacheExecption : Exception
    {
        public CacheExecption(string message) : base(message)
        {

        }
    }
}
