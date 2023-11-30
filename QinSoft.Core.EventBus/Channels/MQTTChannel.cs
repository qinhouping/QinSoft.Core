using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MQTTnet.Client;
using MQTTnet.Protocol;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.MQ.MQTT;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.EventBus.Channels
{
    public class MQTTChannel : Channel
    {
        protected IMqttClient mqttClient { get; set; }

        /// <summary>
        /// MQTT通道主题
        /// </summary>
        public string MQTT_TOPIC { get; set; } = "/QinSoft.Core/__EventBus__";

        /// <summary>
        /// 默认编码
        /// </summary>
        public virtual Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        protected CancellationTokenSource SubscribeCancellationToken = new CancellationTokenSource();

        public MQTTChannel(IMqttClient client) : this(client, NullLoggerFactory.Instance)
        {

        }

        public MQTTChannel(IMqttClient client, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.mqttClient = client;

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMqttClient client, ILoggerFactory loggerFactory) : this(client, loggerFactory.CreateLogger<MQTTChannel>())
        {

        }

        public MQTTChannel(IMQTTManager manager, MQTTChannelOptions options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public MQTTChannel(IMQTTManager manager, MQTTChannelOptions options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Topic, nameof(options.Topic));
            this.mqttClient = string.IsNullOrEmpty(options.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Name);
            this.MQTT_TOPIC = options.Topic;

            this.SubscribeMQTT();
        }

        public MQTTChannel(IMQTTManager manager, IOptions<MQTTChannelOptions> options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public MQTTChannel(IMQTTManager manager, IOptions<MQTTChannelOptions> options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<MQTTChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Value.Topic, nameof(options.Value.Topic));
            this.mqttClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetMqtt() : manager.GetMqtt(options.Value.Name);
            this.MQTT_TOPIC = options.Value.Topic;

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
            this.mqttClient.SubscribeAsync(MQTT_TOPIC, MqttQualityOfServiceLevel.AtMostOnce, this.SubscribeCancellationToken.Token);
        }

        protected override void WriteCore(ChannelData data)
        {
            this.mqttClient.PublishBinaryAsync(MQTT_TOPIC, data.ToJson().ToBytes(DefaultEncoding)).Wait();
        }

        public override void Dispose()
        {
            this.SubscribeCancellationToken.Cancel();
            base.Dispose();
        }
    }

    public class MQTTChannelOptions
    {
        public string Name { get; set; }

        public string Topic { get; set; } = "/QinSoft.Core/__EventBus__";
    }
}
