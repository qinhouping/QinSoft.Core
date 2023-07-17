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
        public static T Get<T>(this IRedisCache redisCache, RedisKey key)
        {
            return redisCache.StringGet(key).ToString().FromJson<T>();
        }

        /// <summary>
        /// 设置键值
        /// </summary>
        public static bool Set<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return redisCache.StringSet(key, value.ToJson());
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        public static T HashGet<T>(this IRedisCache redisCache, RedisKey key, RedisValue field)
        {
            return redisCache.HashGet(key, field).ToString().FromJson<T>();
        }

        /// <summary>
        /// 设置哈希值
        /// </summary>
        public static bool HashSet<T>(this IRedisCache redisCache, RedisKey key, RedisValue field, T value)
        {
            return redisCache.HashSet(key, field, value.ToJson());
        }

        /// <summary>
        /// 获取列表值
        /// </summary>
        public static T ListGet<T>(this IRedisCache redisCache, RedisKey key, long index)
        {
            return redisCache.ListGetByIndex(key, index).ToString().FromJson<T>();
        }

        /// <summary>
        /// 设置列表值
        /// </summary>
        public static void ListSet<T>(this IRedisCache redisCache, RedisKey key, long index, T value)
        {
            redisCache.ListSetByIndex(key, index, value.ToJson());
        }

        /// <summary>
        /// 移除列表值
        /// </summary>
        public static void ListRemove<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            redisCache.ListRemove(key, value.ToJson());
        }

        /// <summary>
        /// 入队列表值
        /// </summary>
        public static long ListPush<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return redisCache.ListRightPush(key, value.ToJson());
        }

        /// <summary>
        /// 出队列表值
        /// </summary>
        public static T ListPop<T>(this IRedisCache redisCache, RedisKey key)
        {
            return redisCache.ListLeftPop(key).ToString().FromJson<T>();
        }

        /// <summary>
        /// 增加集合值
        /// </summary>
        public static bool SetAdd<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return redisCache.SetAdd(key, value.ToJson());
        }

        /// <summary>
        /// 移除集合值
        /// </summary>
        public static bool SetRemove<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return redisCache.SetRemove(key, value.ToJson());
        }

        /// <summary>
        /// 出队集合值
        /// </summary>
        public static T SetPop<T>(this IRedisCache redisCache, RedisKey key)
        {
            return redisCache.SetPop(key).ToString().FromJson<T>();
        }

        /// <summary>
        /// 增加有序集合值
        /// </summary>
        public static bool SortedSetAdd<T>(this IRedisCache redisCache, RedisKey key, T value, double score)
        {
            return redisCache.SortedSetAdd(key, value.ToJson(), score);
        }

        /// <summary>
        /// 移除有序集合值
        /// </summary>
        public static bool SortedSetRemove<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return redisCache.SortedSetRemove(key, value.ToJson());
        }

        /// <summary>
        /// 出队有序集合值
        /// </summary>
        public static T SortedSetPop<T>(this IRedisCache redisCache, RedisKey key, Order order = Order.Ascending)
        {
            return redisCache.SortedSetPop(key, order).ToString().FromJson<T>();
        }

        /// <summary>
        /// 获取键值
        /// </summary>
        public static async Task<T> GetAsync<T>(this IRedisCache redisCache, RedisKey key)
        {
            return (await redisCache.StringGetAsync(key)).ToString().FromJson<T>();
        }

        /// <summary>
        /// 设置键值
        /// </summary>
        public static async Task<bool> SetAsync<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return await redisCache.StringSetAsync(key, value.ToJson());
        }

        /// <summary>
        /// 获取哈希值
        /// </summary>
        public static async Task<T> HashGetAsync<T>(this IRedisCache redisCache, RedisKey key, RedisValue field)
        {
            return (await redisCache.HashGetAsync(key, field)).ToString().FromJson<T>();
        }

        /// <summary>
        /// 设置哈希值
        /// </summary>
        public static async Task<bool> HashSetAsync<T>(this IRedisCache redisCache, RedisKey key, RedisValue field, T value)
        {
            return await redisCache.HashSetAsync(key, field, value.ToJson());
        }

        /// <summary>
        /// 获取列表值
        /// </summary>
        public static async Task<T> ListGetAsync<T>(this IRedisCache redisCache, RedisKey key, long index)
        {
            return (await redisCache.ListGetByIndexAsync(key, index)).ToString().FromJson<T>();
        }

        /// <summary>
        /// 设置列表值
        /// </summary>
        public static async Task ListSetAsync<T>(this IRedisCache redisCache, RedisKey key, long index, T value)
        {
            await redisCache.ListSetByIndexAsync(key, index, value.ToJson());
        }

        /// <summary>
        /// 移除列表值
        /// </summary>
        public static async Task ListRemoveAsync<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            await redisCache.ListRemoveAsync(key, value.ToJson());
        }

        /// <summary>
        /// 入队列表值
        /// </summary>
        public static async Task<long> ListPushAsync<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return await redisCache.ListRightPushAsync(key, value.ToJson());
        }

        /// <summary>
        /// 出队列表值
        /// </summary>
        public static async Task<T> ListPopAsync<T>(this IRedisCache redisCache, RedisKey key)
        {
            return (await redisCache.ListLeftPopAsync(key)).ToString().FromJson<T>();
        }

        /// <summary>
        /// 增加集合值
        /// </summary>
        public static async Task<bool> SetAddAsync<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return await redisCache.SetAddAsync(key, value.ToJson());
        }

        /// <summary>
        /// 移除集合值
        /// </summary>
        public static async Task<bool> SetRemoveAsync<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return await redisCache.SetRemoveAsync(key, value.ToJson());
        }

        /// <summary>
        /// 出队集合值
        /// </summary>
        public static async Task<T> SetPopAsync<T>(this IRedisCache redisCache, RedisKey key)
        {
            return (await redisCache.SetPopAsync(key)).ToString().FromJson<T>();
        }

        /// <summary>
        /// 增加有序集合值
        /// </summary>
        public static async Task<bool> SortedSetAddAsync<T>(this IRedisCache redisCache, RedisKey key, T value, double score)
        {
            return await redisCache.SortedSetAddAsync(key, value.ToJson(), score);
        }

        /// <summary>
        /// 移除有序集合值
        /// </summary>
        public static async Task<bool> SortedSetRemoveAsync<T>(this IRedisCache redisCache, RedisKey key, T value)
        {
            return await redisCache.SortedSetRemoveAsync(key, value.ToJson());
        }

        /// <summary>
        /// 出队有序集合值
        /// </summary>
        public static async Task<T> SortedSetPopAsync<T>(this IRedisCache redisCache, RedisKey key)
        {
            return (await redisCache.SortedSetPopAsync(key)).ToString().FromJson<T>();
        }
    }
}
