using QinSoft.Core.Cache.CSRedis.Core;
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
using StackExchange.Redis;
using System.Net;
using CSRedis;

namespace QinSoft.Core.Cache.CSRedis
{
    /// <summary>
    /// CSRedis缓存管理器
    /// 集成缓存配置，支持多数据源
    /// 支持单机，哨兵和自支持集群模式
    /// </summary>
    public class CSRedisCacheManager : ICSRedisCacheManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, CSRedisCache> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Redis缓存配置
        /// </summary>
        public CSRedisCacheManagerConfig CSRedisCacheManagerConfig { get; private set; }


        public CSRedisCacheManager(CSRedisCacheManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<CSRedisCacheManager>())
        {
        }

        public CSRedisCacheManager(CSRedisCacheManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, nameof(config));
            ObjectUtils.CheckNull(logger, nameof(logger));
            CacheDictionary = new ConcurrentDictionary<string, CSRedisCache>();
            CSRedisCacheManagerConfig = config;
            this.logger = logger;
        }

        public CSRedisCacheManager(CSRedisCacheManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public CSRedisCacheManager(CSRedisCacheManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            CacheDictionary = new ConcurrentDictionary<string, CSRedisCache>();
            CSRedisCacheManagerConfig = configer.Get<CSRedisCacheManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<CSRedisCacheManager>();
        }

        public CSRedisCacheManager(IOptions<CSRedisCacheManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public CSRedisCacheManager(IOptions<CSRedisCacheManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, nameof(optionsAccessor));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            CacheDictionary = new ConcurrentDictionary<string, CSRedisCache>();
            CSRedisCacheManagerConfig = configer.Get<CSRedisCacheManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<CSRedisCacheManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual CSRedisCacheItemConfig GetCacheItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return CSRedisCacheManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual CSRedisCacheItemConfig GetDefaultCacheItemConfig()
        {
            return GetCacheItemConfig(this.CSRedisCacheManagerConfig.Primary);
        }

        /// <summary>
        /// 构建缓存实例
        /// </summary>
        protected virtual CSRedisCache BuildCacheFromConfig(CSRedisCacheItemConfig config)
        {
            ObjectUtils.CheckNull(config, nameof(config));
            if (config.ConnectionStrings.IsNotEmpty())
            {
                IKeyRule keyRule = null;
                if (config.KeyRuleType != null)
                {
                    keyRule = Activator.CreateInstance(Type.GetType(config.KeyRuleType)) as IKeyRule;
                    keyRule.ConnectionStrings = config.ConnectionStrings;
                }
                return new CSRedisCache(keyRule == null ? null : new Func<string, string>(keyRule.GetKey), config.ConnectionStrings);
            }
            else if (config.Sentinels.IsNotEmpty())
            {
                return new CSRedisCache(config.ConnectionString, config.Sentinels);
            }
            return new CSRedisCache(config.ConnectionString);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual CSRedisCache GetCache()
        {
            CSRedisCacheItemConfig config = GetDefaultCacheItemConfig();
            if (config == null)
            {
                throw new CacheExecption("not found default cache config");
            }

            CSRedisCache cache = CacheDictionary.GetOrAdd(config.Name, key =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug("get default cache from config");

            return cache;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<CSRedisCache> GetCacheAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetCache);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual CSRedisCache GetCache(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            CSRedisCacheItemConfig config = GetCacheItemConfig(name);
            if (config == null)
            {
                throw new CacheExecption(string.Format("not found cache config:{0}", name));
            }

            CSRedisCache cache = CacheDictionary.GetOrAdd(config.Name, key =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug(string.Format("get cache from config:{0}", name));

            return cache;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<CSRedisCache> GetCacheAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetCache, name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, CSRedisCache> pair in CacheDictionary)
                {
                    pair.Value.Dispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
