using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.MQ.Kafka;
using QinSoft.Core.MQ.Kafka.Core;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Text;
using System.Threading;

namespace QinSoft.Core.EventBus.Channels
{
    public class KafkaChannel : Channel
    {
        protected IKafkaClient<byte[], byte[]> KafkaClient { get; set; }

        /// <summary>
        /// Kafka通道主题
        /// </summary>
        public string Kafka_TOPIC { get; set; } = "QinSoft.Core.__EventBus__";

        /// <summary>
        /// 默认编码
        /// </summary>
        public virtual Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        protected CancellationTokenSource SubscribeCancellationToken = new CancellationTokenSource();

        public KafkaChannel(IKafkaClient<byte[], byte[]> client) : this(client, NullLoggerFactory.Instance)
        {

        }

        public KafkaChannel(IKafkaClient<byte[], byte[]> client, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.KafkaClient = client;

            this.SubscribeKafka();
        }

        public KafkaChannel(IKafkaClient<byte[], byte[]> client, ILoggerFactory loggerFactory) : this(client, loggerFactory.CreateLogger<KafkaChannel>())
        {

        }

        public KafkaChannel(IKafkaManager manager, KafkaChannelOptions options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public KafkaChannel(IKafkaManager manager, KafkaChannelOptions options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<KafkaChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Topic, nameof(options.Topic));
            this.KafkaClient = string.IsNullOrEmpty(options.Name) ? manager.GetKafka<byte[], byte[]>() : manager.GetKafka<byte[], byte[]>(options.Name);
            this.Kafka_TOPIC = options.Topic;

            this.SubscribeKafka();
        }

        public KafkaChannel(IKafkaManager manager, IOptions<KafkaChannelOptions> options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public KafkaChannel(IKafkaManager manager, IOptions<KafkaChannelOptions> options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<KafkaChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Value.Topic, nameof(options.Value.Topic));
            this.KafkaClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetKafka<byte[], byte[]>() : manager.GetKafka<byte[], byte[]>(options.Value.Name);
            this.Kafka_TOPIC = options.Value.Topic;

            this.SubscribeKafka();
        }

        /// <summary>
        /// 订阅Kafka
        /// </summary>
        protected virtual void SubscribeKafka()
        {
            this.KafkaClient.Consume(Kafka_TOPIC, (c, r) =>
            {
                this.Read(JsonUtils.FromJson<ChannelData>(r.Message.Value.ToString(DefaultEncoding)));
            }, this.SubscribeCancellationToken.Token);
        }

        protected override void WriteCore(ChannelData data)
        {
            this.KafkaClient.Produce(Kafka_TOPIC, data.Id.ToBytes(DefaultEncoding), data.ToJson().ToBytes(DefaultEncoding));
        }

        public override void Dispose()
        {
            this.SubscribeCancellationToken.Cancel();
            base.Dispose();
        }
    }

    public class KafkaChannelOptions
    {
        public string Name { get; set; }

        public string Topic { get; set; } = "QinSoft.Core.__EventBus__";
    }
}
