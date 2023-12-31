﻿using QinSoft.Core.Cache;
using QinSoft.Core.Cache.Local;
using QinSoft.Core.Cache.Redis;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.Cache.CSRedis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 缓存依赖注入扩展
    /// </summary>
    public static class CacheServiceCollectionExtensions
    {
        /// <summary>
        /// 注入本地缓存
        /// </summary>
        public static IServiceCollection AddLocalCacheManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<ILocalCacheManager, LocalCacheManager>());
            services.Configure((LocalCacheManagerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入本地缓存
        /// </summary>
        public static IServiceCollection AddLocalCacheManager(this IServiceCollection services, Action<LocalCacheManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(setupAction, nameof(setupAction));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<ILocalCacheManager, LocalCacheManager>());
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 注入Redis缓存
        /// </summary>
        public static IServiceCollection AddRedisCacheManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRedisCacheManager, RedisCacheManager>());
            services.Configure((RedisCacheManagerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入Redis缓存
        /// </summary>
        public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, Action<RedisCacheManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(setupAction, nameof(setupAction));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRedisCacheManager, RedisCacheManager>());
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 注入CSRedis缓存
        /// </summary>
        public static IServiceCollection AddCSRedisCacheManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<ICSRedisCacheManager, CSRedisCacheManager>());
            services.Configure((CSRedisCacheManagerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入CSRedis缓存
        /// </summary>
        public static IServiceCollection AddCSRedisCacheManager(this IServiceCollection services, Action<CSRedisCacheManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(setupAction, nameof(setupAction));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<ICSRedisCacheManager, CSRedisCacheManager>());
            services.Configure(setupAction);
            return services;
        }
    }
}
