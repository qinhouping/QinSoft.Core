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

        /// <summary>
        /// 安全资源释放
        /// </summary>
        public virtual void SafeDispose()
        {
            base.Dispose();
        }

        /// <summary>
        /// 资源释放，覆盖原有资源释放防止使用using产生异常
        /// </summary>
        public new virtual void Dispose()
        {
        }
    }
}
