using QinSoft.Core.Common.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.MQ.Kafka;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 消息队列服务依赖注入扩展
    /// </summary>
    public static class MQServiceCollectionExtensions
    {
        /// <summary>
        /// 注入Kafka
        /// </summary>
        public static IServiceCollection AddKafkaManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IKafkaManager, KafkaManager>());
            return services;
        }

        /// <summary>
        /// 注入Database
        /// </summary>
        public static IServiceCollection AddKafkaManager(this IServiceCollection services, Action<KafkaManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, "services");
            ObjectUtils.CheckNull(setupAction, "setupAction");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IKafkaManager, KafkaManager>());
            services.Configure(setupAction);
            return services;
        }
    }
}
