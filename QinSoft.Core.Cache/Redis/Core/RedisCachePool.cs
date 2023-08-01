using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using QinSoft.Core.Common.Utils;
using StackExchange.Redis.MultiplexerPool;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存实现
    /// </summary>
    internal class RedisCachePool : IRedisCachePool
    {
        protected IConnectionMultiplexerPool ConnectionMultiplexerPool { get; set; }

        public RedisCachePool(int poolSize, RedisCacheOptions options, ConnectionSelectionStrategy pooStrategy = ConnectionSelectionStrategy.RoundRobin)
        {
            ObjectUtils.CheckNull(options, "options");
            if (string.IsNullOrEmpty(options.Configuration))
            {
                ConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(poolSize, options.ConfigurationOptions, null, pooStrategy);
            }
            else
            {
                ConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(poolSize, options.Configuration, null, pooStrategy);
            }
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
