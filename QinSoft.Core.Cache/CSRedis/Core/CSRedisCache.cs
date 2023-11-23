using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.CSRedis.Core
{
    public class CSRedisCache : CSRedisClient, ICache
    {
        public CSRedisCache(string connectionString) : base(connectionString)
        {
        }

        public CSRedisCache(Func<string, string> NodeRule, params string[] connectionStrings) : base(NodeRule, connectionStrings)
        {
        }

        public CSRedisCache(string connectionString, string[] sentinels, bool readOnly = false) : base(connectionString, sentinels, readOnly)
        {
        }

        public CSRedisCache(string connectionString, string[] sentinels, bool readOnly, SentinelMasterConverter convert) : base(connectionString, sentinels, readOnly, convert)
        {
        }

        protected CSRedisCache(Func<string, string> NodeRule, string[] sentinels, bool readOnly, SentinelMasterConverter convert = null, params string[] connectionStrings) : base(NodeRule, sentinels, readOnly, convert, connectionStrings)
        {
        }

        public bool Delete(string key)
        {
            return this.Del(key) > 0;
        }

        public T GetT<T>(string key, Func<string, T> getValue = null, TimeSpan? timeSpan = null) where T : class
        {
            if (this.Exists(key))
            {
                return this.Get<T>(key);
            }
            else
            {
                if (getValue != null)
                {
                    T value = getValue(key);
                    this.Set(key, value, timeSpan);
                    return value;
                }
                return null;
            }
        }

        public bool Set<T>(string key, T value, TimeSpan? timeSpan = null) where T : class
        {
            return this.Set(key, value, timeSpan != null ? (int)(timeSpan.Value.TotalSeconds) : -1);
        }
    }
}
