using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Local.Core
{
    /// <summary>
    /// 本地缓存实现
    /// </summary>
    internal class LocalCache : MemoryCache, ILocalCache
    {
        public LocalCache(LocalCacheOptions options) : base(options)
        {
        }
    }
}
