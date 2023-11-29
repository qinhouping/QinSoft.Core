using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QinSoft.Core.Common.Utils;
using StackExchange.Redis;
using StackExchange.Redis.MultiplexerPool;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存连接池实现
    /// </summary>
    internal class RedisCachePool : IRedisCachePool
    {
        protected IConnectionMultiplexerPool ConnectionMultiplexerPool { get; set; }

        public RedisCachePool(int poolSize, ConfigurationOptions configurationOptions, ConnectionSelectionStrategy connectionSelectionStrategy = ConnectionSelectionStrategy.RoundRobin)
        {
            ObjectUtils.CheckNull(configurationOptions, nameof(configurationOptions));
            ConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(poolSize, configurationOptions, null, connectionSelectionStrategy);
        }

        public RedisCachePool(int poolSize, string configuration, ConnectionSelectionStrategy connectionSelectionStrategy = ConnectionSelectionStrategy.RoundRobin)
        {
            ObjectUtils.CheckNull(configuration, nameof(configuration));
            ConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(poolSize, configuration, null, connectionSelectionStrategy);
        }

        public RedisCachePool(RedisCachePoolOptions redisCachePoolOptions)
        {
            ObjectUtils.CheckNull(redisCachePoolOptions, nameof(redisCachePoolOptions));
            ConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(redisCachePoolOptions.PoolSize, redisCachePoolOptions.ConfigurationOptions, redisCachePoolOptions.TextWriter, redisCachePoolOptions.ConnectionSelectionStrategy);
        }

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            ConnectionMultiplexerPool.Dispose();
        }

        public virtual IRedisCache Get()
        {
            return new RedisCache(ConnectionMultiplexerPool.GetAsync().Result);
        }

        public virtual async Task<IRedisCache> GetAsync()
        {
            return new RedisCache(await ConnectionMultiplexerPool.GetAsync());
        }
    }
}
