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

        public bool Delete(string key)
        {
            this.Remove(key);
            return true;
        }

        public T GetT<T>(string key, Func<string, T> getValue = null, TimeSpan? timeSpan = null) where T : class
        {
            T value = default;
            if (this.TryGetValue<T>(key, out value))
            {
                return value;
            }
            else
            {
                value = getValue(key);
                this.Set<T>(key, value, timeSpan);
                return value;
            }
        }

        public bool Set<T>(string key, T value, TimeSpan? timeSpan = null) where T : class
        {
            if (timeSpan != null)
            {
                CacheExtensions.Set<T>(this, key, value);
            }
            else
            {
                CacheExtensions.Set<T>(this, key, value, timeSpan.Value);
            }
            return true;
        }
    }
}
