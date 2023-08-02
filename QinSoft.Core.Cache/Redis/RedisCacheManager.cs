using QinSoft.Core.Cache.Redis.Core;
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

namespace QinSoft.Core.Cache.Redis
{
    /// <summary>
    /// Redis缓存管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class RedisCacheManager : IRedisCacheManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IRedisCache> CacheDictionary;

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
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            CacheDictionary = new ConcurrentDictionary<string, IRedisCache>();
            RedisCacheManagerConfig = config;
            this.logger = logger;
        }

        public RedisCacheManager(RedisCacheManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public RedisCacheManager(RedisCacheManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IRedisCache>();
            RedisCacheManagerConfig = configer.Get<RedisCacheManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<RedisCacheManager>();
        }

        public RedisCacheManager(IOptions<RedisCacheManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public RedisCacheManager(IOptions<RedisCacheManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IRedisCache>();
            RedisCacheManagerConfig = configer.Get<RedisCacheManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<RedisCacheManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual RedisCacheItemConfig GetCacheItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
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
        /// 获取redis配置
        /// </summary>
        protected virtual RedisCacheOptions GetRedisCacheOptions(RedisCacheItemConfig config)
        {
            RedisCacheOptions options = new RedisCacheOptions();
            options.ConfigurationOptions = new ConfigurationOptions();
            config.EndPoints?.ToList().ForEach(u =>
            {
                options.ConfigurationOptions.EndPoints.Add(u);
            });
            options.ConfigurationOptions.Password = config.Passowrd;
            options.ConfigurationOptions.DefaultDatabase = config.DefaultDatabase;
            options.ConfigurationOptions.ConnectTimeout = config.ConnectTimeout;
            options.ConfigurationOptions.AllowAdmin = true;
            //哨兵模式
            if (config.Sentinel != null)
            {
                options.ConfigurationOptions.EndPoints.Clear();
                config.Sentinel.EndPoints?.ToList().ForEach(u =>
                {
                    options.ConfigurationOptions.EndPoints.Add(u);
                });
                options.ConfigurationOptions.ServiceName = config.Sentinel.ServiceName;
                options.IsSentinel = true;
            }
            return options;
        }

        /// <summary>
        /// 构建缓存实例
        /// </summary>
        protected virtual IRedisCache BuildCacheFromConfig(RedisCacheItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            return new RedisCache(GetRedisCacheOptions(config));
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

            IRedisCache cache = CacheDictionary.GetOrAdd(config.Name, (key) =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug("get default cache from config");

            return cache;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<IRedisCache> GetCacheAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetCache);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual IRedisCache GetCache(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            RedisCacheItemConfig config = GetCacheItemConfig(name);
            if (config == null)
            {
                throw new CacheExecption(string.Format("not found cache config:{0}", name));
            }

            IRedisCache cache = CacheDictionary.GetOrAdd(config.Name, (key) =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug(string.Format("get cache from config:{0}", name));

            return cache;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<IRedisCache> GetCacheAsync(string name)
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
                foreach (KeyValuePair<string, IRedisCache> pair in CacheDictionary)
                {
                    pair.Value.SafeDispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
