using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.Common.Utils;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存扩展类
    /// </summary>
    public static class RedisCacheExtensions
    {
        /// <summary>
        /// 获取键值
        /// </summary>
        public static RedisValue StringGetOrAdd(this IRedisCache redisCache, RedisKey key, Func<RedisKey, RedisValue> func, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            lock (redisCache)
            {
                bool exists = redisCache.KeyExists(key, flags);
                if (!exists)
                {
                    RedisValue value = func(key);
                    redisCache.StringSet(key, value, expiry, When.NotExists, flags);
                }
                return redisCache.StringGet(key, flags);
            }
        }

        /// <summary>
        /// 获取键值
        /// </summary>
        public static async Task<RedisValue> StringGetOrAddAsycn(this IRedisCache redisCache, RedisKey key, Func<RedisKey, RedisValue> func, TimeSpan? expiry = null, CommandFlags flags = CommandFlags.None)
        {
            return await ExecuteUtils.ExecuteInTask(() => StringGetOrAdd(redisCache, key, func, expiry, flags));
        }

        /// <summary>
        /// 获取哈希键值
        /// </summary>
        public static RedisValue HashGetOrAdd(this IRedisCache redisCache, RedisKey key, RedisValue field, Func<RedisKey, RedisValue, RedisValue> func, CommandFlags flags = CommandFlags.None)
        {
            lock (redisCache)
            {
                bool exists = redisCache.HashExists(key, field, flags);
                if (!exists)
                {
                    RedisValue value = func(key, field);
                    redisCache.HashSet(key, field, value, When.NotExists, flags);
                }
                return redisCache.HashGet(key, field, flags);
            }
        }

        /// <summary>
        /// 获取哈希键值
        /// </summary>
        public static async Task<RedisValue> HashGetOrAddAsync(this IRedisCache redisCache, RedisKey key, RedisValue field, Func<RedisKey, RedisValue, RedisValue> func, CommandFlags flags = CommandFlags.None)
        {
            return await ExecuteUtils.ExecuteInTask(() => HashGetOrAdd(redisCache, key, field, func, flags));
        }
    }
}
