using QinSoft.Core.Common.Utils;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.MQ.Kafka;
using QinSoft.Core.MQ.RabbitMQ;
using QinSoft.Core.MQ.MQTT;
using QinSoft.Core.EventBus;
using QinSoft.Core.EventBus.Channels;
using QinSoft.Core.EventBus.Publishers;
using QinSoft.Core.EventBus.Subscriber;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 事件总线服务依赖注入扩展
    /// </summary>
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// 注入EventBus
        /// </summary>
        public static IServiceCollection AddEventBus(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IPublisher, SimplePublisher>());
            services.TryAdd(ServiceDescriptor.Singleton<ISubscriber, SimpleSubscriber>());
            AddMemoryChannel(new EventBusBuilder(services));
            return services;
        }

        /// <summary>
        /// 注入EventBus
        /// </summary>
        public static IServiceCollection AddEventBus(this IServiceCollection services, Action<EventBusBuilder> builder)
        {
            ObjectUtils.CheckNull(services, nameof(services));
            ObjectUtils.CheckNull(builder, nameof(builder));
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IPublisher, SimplePublisher>());
            services.TryAdd(ServiceDescriptor.Singleton<ISubscriber, SimpleSubscriber>());
            builder(new EventBusBuilder(services));
            return services;
        }

        /// <summary>
        /// 注入内存通道
        /// </summary>
        public static EventBusBuilder AddMemoryChannel(this EventBusBuilder builder)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, MemoryChannel>());
            builder.Services.Configure((MQTTChannelOptions options) => { });
            return builder;
        }

        /// <summary>
        /// 注入MQTT通道
        /// </summary>
        public static EventBusBuilder AddMQTTChannel(this EventBusBuilder builder)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, MQTTChannel>());
            builder.Services.Configure((MQTTChannelOptions options) => { });
            return builder;
        }

        /// <summary>
        /// 注入MQTT通道
        /// </summary>
        public static EventBusBuilder AddMQTTChannel(this EventBusBuilder builder, Action<MQTTChannelOptions> setupAction)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            ObjectUtils.CheckNull(setupAction, nameof(setupAction));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, MQTTChannel>());
            builder.Services.Configure(setupAction);
            return builder;
        }

        /// <summary>
        /// 注入Kafka通道
        /// </summary>
        public static EventBusBuilder AddKafkaChannel(this EventBusBuilder builder)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, KafkaChannel>());
            builder.Services.Configure((KafkaChannelOptions options) => { });
            return builder;
        }

        /// <summary>
        /// 注入Kafka通道
        /// </summary>
        public static EventBusBuilder AddKafkaChannel(this EventBusBuilder builder, Action<KafkaChannelOptions> setupAction)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            ObjectUtils.CheckNull(setupAction, nameof(setupAction));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, KafkaChannel>());
            builder.Services.Configure(setupAction);
            return builder;
        }

        /// <summary>
        /// 注入Kafka通道
        /// </summary>
        public static EventBusBuilder AddRabbitMQChannel(this EventBusBuilder builder)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, RabbitMQChannel>());
            builder.Services.Configure((RabbitMQChannelOptions options) => { });
            return builder;
        }

        /// <summary>
        /// 注入Kafka通道
        /// </summary>
        public static EventBusBuilder AddRabbitMQChannel(this EventBusBuilder builder, Action<RabbitMQChannelOptions> setupAction)
        {
            ObjectUtils.CheckNull(builder, nameof(builder));
            ObjectUtils.CheckNull(setupAction, nameof(setupAction));
            builder.Services.TryAdd(ServiceDescriptor.Singleton<Channel, RabbitMQChannel>());
            builder.Services.Configure(setupAction);
            return builder;
        }
    }

    public class EventBusBuilder
    {
        public IServiceCollection Services { get; private set; }
        public EventBusBuilder(IServiceCollection services)
        {
            this.Services = services;
        }
    }
}
