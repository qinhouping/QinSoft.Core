using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 配置器依赖注入扩展
    /// </summary>
    public static class ConfigerServiceCollectionExtensions
    {
        /// <summary>
        /// 注入文件配置
        /// </summary>
        public static IServiceCollection AddFileConfiger(this IServiceCollection services)
        {
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IConfiger, FileConfiger>());
            services.Configure((FileConfigerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入文件配置
        /// </summary>
        public static IServiceCollection AddFileConfiger(this IServiceCollection services, Action<FileConfigerOptions> setupAction)
        {
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IConfiger, FileConfiger>());
            services.Configure(setupAction);
            return services;
        }
    }
}
