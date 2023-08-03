using QinSoft.Core.Cache.Local.Core;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Local
{
    /// <summary>
    /// 本地缓存管理器实现
    /// </summary>
    public class LocalCacheManager : ILocalCacheManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, ILocalCache> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// 本地缓存配置
        /// </summary>
        protected LocalCacheManagerConfig LocalCacheManagerConfig { get; private set; }

        public LocalCacheManager(LocalCacheManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<LocalCacheManager>())
        {
        }

        public LocalCacheManager(LocalCacheManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            CacheDictionary = new ConcurrentDictionary<string, ILocalCache>();
            LocalCacheManagerConfig = config;
            this.logger = logger;
        }

        public LocalCacheManager(LocalCacheManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public LocalCacheManager(LocalCacheManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, ILocalCache>();
            LocalCacheManagerConfig = configer.Get<LocalCacheManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<LocalCacheManager>();
        }

        public LocalCacheManager(IOptions<LocalCacheManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public LocalCacheManager(IOptions<LocalCacheManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, ILocalCache>();
            LocalCacheManagerConfig = configer.Get<LocalCacheManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<LocalCacheManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual LocalCacheItemConfig GetCacheItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            return LocalCacheManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual LocalCacheItemConfig GetDefaultCacheItemConfig()
        {
            return new LocalCacheItemConfig();
        }

        /// <summary>
        /// 构建缓存实例
        /// </summary>
        protected virtual ILocalCache BuildCacheFromConfig(LocalCacheItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            return new LocalCache(new LocalCacheOptions()
            {
                ExpirationScanFrequency = config.ExpirationScanFrequency,
                SizeLimit = config.SizeLimie,
                CompactionPercentage = config.CompactionPercentage
            });
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual ILocalCache GetCache()
        {
            LocalCacheItemConfig config = GetDefaultCacheItemConfig();
            if (config == null)
            {
                throw new CacheExecption("not found default cache config");
            }

            ILocalCache cache = CacheDictionary.GetOrAdd(config.Name, key =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug("get default cache from config");

            return cache;
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<ILocalCache> GetCacheAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetCache);
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual ILocalCache GetCache(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            LocalCacheItemConfig config = GetCacheItemConfig(name);
            if (config == null)
            {
                throw new CacheExecption(string.Format("not found cache config:{0}", name));
            }

            ILocalCache cache = CacheDictionary.GetOrAdd(config.Name, key =>
            {
                return BuildCacheFromConfig(config);
            });

            logger.LogDebug(string.Format("get cache from config:{0}", name));

            return cache;
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        public virtual async Task<ILocalCache> GetCacheAsync(string name)
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
                foreach (KeyValuePair<string, ILocalCache> pair in CacheDictionary)
                {
                    pair.Value.SafeDispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
