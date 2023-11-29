using QinSoft.Core.Common.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.MQ.Kafka;
using QinSoft.Core.MQ.RabbitMQ;
using QinSoft.Core.MQ.MQTT;

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
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IKafkaManager, KafkaManager>());
            services.Configure((KafkaManagerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入Kafka
        /// </summary>
        public static IServiceCollection AddKafkaManager(this IServiceCollection services, Action<KafkaManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(services, nameof(setupAction));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IKafkaManager, KafkaManager>());
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 注入RabbitMQ
        /// </summary>
        public static IServiceCollection AddRabbitMQManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRabbitMQManager, RabbitMQManager>());
            services.Configure((RabbitMQManagerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入RabbitMQ
        /// </summary>
        public static IServiceCollection AddRabbitMQManager(this IServiceCollection services, Action<RabbitMQManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(services, nameof(setupAction));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IRabbitMQManager, RabbitMQManager>());
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 注入MQTT
        /// </summary>
        public static IServiceCollection AddMQTTManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMQTTManager, MQTTManager>());
            services.Configure((MQTTManagerOptions options) => { });
            return services;
        }

        /// <summary>
        /// 注入MQTT
        /// </summary>
        public static IServiceCollection AddMQTTManager(this IServiceCollection services, Action<MQTTManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(services, nameof(setupAction));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMQTTManager, MQTTManager>());
            services.Configure(setupAction);
            return services;
        }
    }
}
