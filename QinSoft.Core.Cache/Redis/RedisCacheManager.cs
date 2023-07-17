﻿using QinSoft.Core.Cache.Redis.Core;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Redis
{
    /// <summary>
    /// Redis缓存管理器
    /// 集成缓存配置，支持多数据源，支持连接池
    /// </summary>
    public class RedisCacheManager : IRedisCacheManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IRedisCachePool> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Redis缓存配置
        /// </summary>
        public RedisCacheManagerConfig RedisCacheManagerConfig { get; private set; }


        public RedisCacheManager(RedisCacheManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<RedisCacheManager>())
        {
        }

        public RedisCacheManager(RedisCacheManagerConfig config, ILogger logger)
        {
            ArgumentUtils.CheckNull(config, "config");
            ArgumentUtils.CheckNull(logger, "logger");
            CacheDictionary = new ConcurrentDictionary<string, IRedisCachePool>();
            RedisCacheManagerConfig = config;
            this.logger = logger;
        }

        public RedisCacheManager(RedisCacheManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public RedisCacheManager(RedisCacheManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ArgumentUtils.CheckNull(options, "options");
            ArgumentUtils.CheckNull(configer, "configer");
            ArgumentUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IRedisCachePool>();
            RedisCacheManagerConfig = configer.Get<RedisCacheManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<RedisCacheManager>();
        }

        public RedisCacheManager(IOptions<RedisCacheManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public RedisCacheManager(IOptions<RedisCacheManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ArgumentUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ArgumentUtils.CheckNull(configer, "configer");
            ArgumentUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IRedisCachePool>();
            RedisCacheManagerConfig = configer.Get<RedisCacheManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<RedisCacheManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual RedisCacheItemConfig GetCacheItemConfig(string name)
        {
            ArgumentUtils.CheckNull(name, name);
            return RedisCacheManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual RedisCacheItemConfig GetDefaultCacheItemConfig()
        {
            return GetCacheItemConfig(this.RedisCacheManagerConfig.Primary);
        }

        /// <summary>
        /// 构建缓存实例
        /// </summary>
        protected IRedisCachePool BuildCacheFromConfig(RedisCacheItemConfig config)
        {
            return new BackupableRedisCachePool(config.PoolSize, new RedisCacheOptions()
            {
                Configuration = config.Configuration
            }, config.Backups?.Select(u => new RedisCacheOptions()
            {
                Configuration = u.Configuration
            }).ToArray());
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual IRedisCache GetCache()
        {
            RedisCacheItemConfig config = GetDefaultCacheItemConfig();
            if (config == null)
            {
                throw new CacheExecption("not found default cache config");
            }

            IRedisCachePool pool = CacheDictionary.GetOrAdd(config.Name, (key) =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug("get default cache from config");

            return pool.Get();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<IRedisCache> GetCacheAsync()
        {
            RedisCacheItemConfig config = GetDefaultCacheItemConfig();
            if (config == null)
            {
                throw new CacheExecption("not found default cache config");
            }

            IRedisCachePool pool = CacheDictionary.GetOrAdd(config.Name, (key) =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug("get default cache from config");

            return await pool.GetAsync();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual IRedisCache GetCache(string name)
        {
            ArgumentUtils.CheckNull(name, "name");
            RedisCacheItemConfig config = GetCacheItemConfig(name);
            if (config == null)
            {
                throw new CacheExecption(string.Format("not found cache config:{0}", name));
            }

            IRedisCachePool pool = CacheDictionary.GetOrAdd(config.Name, (key) =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug(string.Format("get cache from config:{0}", name));

            return pool.Get();
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<IRedisCache> GetCacheAsync(string name)
        {
            ArgumentUtils.CheckNull(name, "name");
            RedisCacheItemConfig config = GetCacheItemConfig(name);
            if (config == null)
            {
                throw new CacheExecption(string.Format("not found cache config:{0}", name));
            }

            IRedisCachePool pool = CacheDictionary.GetOrAdd(config.Name, (key) =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug(string.Format("get cache from config:{0}", name));

            return await pool.GetAsync();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, IRedisCachePool> pair in CacheDictionary)
                {
                    pair.Value.Dispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
