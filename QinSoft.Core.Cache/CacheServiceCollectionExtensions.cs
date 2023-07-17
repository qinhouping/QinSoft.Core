using QinSoft.Core.Cache;
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
            ArgumentUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<ILocalCacheManager, LocalCacheManager>());
            return services;
        }

        /// <summary>
        /// 注入本地缓存
        /// </summary>
        public static IServiceCollection AddLocalCacheManager(this IServiceCollection services, Action<LocalCacheManagerOptions> setupAction)
        {
            ArgumentUtils.CheckNull(services, "services");
            ArgumentUtils.CheckNull(setupAction, "setupAction");
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
            ArgumentUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRedisCacheManager, RedisCacheManager>());
            return services;
        }

        /// <summary>
        /// 注入Redis缓存
        /// </summary>
        public static IServiceCollection AddRedisCacheManager(this IServiceCollection services, Action<RedisCacheManagerOptions> setupAction)
        {
            ArgumentUtils.CheckNull(services, "services");
            ArgumentUtils.CheckNull(setupAction, "setupAction");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRedisCacheManager, RedisCacheManager>());
            services.Configure(setupAction);
            return services;
        }
    }
}
