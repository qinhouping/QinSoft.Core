using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QinSoft.Core.Common.Utils;
using StackExchange.Redis.MultiplexerPool;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// 可备份的Redis缓存池，支持多写
    /// </summary>
    internal class BackupableRedisCachePool : RedisCachePool
    {
        /// <summary>
        /// 备份连接池
        /// </summary>
        protected IList<IConnectionMultiplexerPool> BackupConnectionMultiplexerPools { get; set; }

        public BackupableRedisCachePool(int poolSize, RedisCacheOptions options, RedisCacheOptions[] backupOptions, ConnectionSelectionStrategy pooStrategy = ConnectionSelectionStrategy.RoundRobin) : base(poolSize, options, pooStrategy)
        {
            BackupConnectionMultiplexerPools = new List<IConnectionMultiplexerPool>();
            backupOptions = backupOptions ?? new RedisCacheOptions[0];
            foreach (RedisCacheOptions backupOpt in backupOptions)
            {
                IConnectionMultiplexerPool backupConnectionMultiplexerPool = null;
                if (string.IsNullOrEmpty(backupOpt.Configuration))
                {
                    backupConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(poolSize, backupOpt.ConfigurationOptions, null, pooStrategy);
                }
                else
                {
                    backupConnectionMultiplexerPool = ConnectionMultiplexerPoolFactory.Create(poolSize, backupOpt.Configuration, null, pooStrategy);
                }
                BackupConnectionMultiplexerPools.Add(backupConnectionMultiplexerPool);
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            foreach (IConnectionMultiplexerPool backupConnectionPool in BackupConnectionMultiplexerPools)
            {
                backupConnectionPool.Dispose();
            }
            ConnectionMultiplexerPool.Dispose();
        }

        /// <summary>
        /// 获取Redis缓存
        /// </summary>
        public override IRedisCache Get()
        {
            IReconnectableConnectionMultiplexer main = ConnectionMultiplexerPool.GetAsync().Result;
            IList<IReconnectableConnectionMultiplexer> backups = new List<IReconnectableConnectionMultiplexer>();
            foreach (IConnectionMultiplexerPool backupPool in BackupConnectionMultiplexerPools)
            {
                backups.Add(backupPool.GetAsync().Result);
            }
            return new BackupableRedisCache(main, backups.ToArray());
        }

        /// <summary>
        /// 获取Redis缓存
        /// </summary>
        public override async Task<IRedisCache> GetAsync()
        {
            IReconnectableConnectionMultiplexer main = await ConnectionMultiplexerPool.GetAsync();
            IList<IReconnectableConnectionMultiplexer> backups = new List<IReconnectableConnectionMultiplexer>();
            foreach (IConnectionMultiplexerPool backupPool in BackupConnectionMultiplexerPools)
            {
                backups.Add(await backupPool.GetAsync());
            }
            return new BackupableRedisCache(main, backups.ToArray());
        }
    }
}
