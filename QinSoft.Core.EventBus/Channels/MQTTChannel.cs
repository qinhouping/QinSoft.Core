using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.MQ.MQTT;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.EventBus.Channels
{
    public class MQTTChannel : Channel
    {
        protected IMqttClient mqttClient { get; set; }

        protected string MQTT_TOPIC = "/QinSoft.Core/__EventBus__";

        /// <summary>
        /// 默认编码
        /// </summary>
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        public MQTTChannel(IMqttClient client) : base(NullLoggerFactory.Instance.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.mqttClient = client;

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMqttClient client, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.mqttClient = client;

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMqttClient client, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.mqttClient = client;

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, MQTTChannelOptions options) : base(NullLoggerFactory.Instance.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            this.mqttClient = string.IsNullOrEmpty(options.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Name);

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, MQTTChannelOptions options, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            this.mqttClient = string.IsNullOrEmpty(options.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Name);

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, MQTTChannelOptions options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            this.mqttClient = string.IsNullOrEmpty(options.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Name);

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, IOptions<MQTTChannelOptions> options) : base(NullLoggerFactory.Instance.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            this.mqttClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Value.Name);

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, IOptions<MQTTChannelOptions> options, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            this.mqttClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Value.Name);

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, IOptions<MQTTChannelOptions> options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            this.mqttClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Value.Name);

            this.SubscribeMQTT();
        }

        /// <summary>
        /// 订阅MQTT
        /// </summary>
        protected virtual void SubscribeMQTT()
        {
            this.mqttClient.ApplicationMessageReceivedAsync += async e =>
            {
                this.Read(JsonUtils.FromJson<ChannelData>(e.ApplicationMessage.PayloadSegment.ToArray().ToString(DefaultEncoding)));
            };
            this.mqttClient.SubscribeAsync(this.MQTT_TOPIC);
        }

        protected override void WriteCore(ChannelData data)
        {
            this.mqttClient.PublishBinaryAsync(MQTT_TOPIC, data.ToJson().ToBytes(DefaultEncoding)).Wait();
        }
    }

    public class MQTTChannelOptions
    {
        public string Name { get; set; }
    }
}
