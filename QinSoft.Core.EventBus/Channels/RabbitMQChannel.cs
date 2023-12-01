using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.MQ.RabbitMQ;
using QinSoft.Core.MQ.RabbitMQ.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.EventBus.Channels
{
    public class RabbitMQChannel : Channel
    {
        protected IRabbitMQClient RabbitMQClient { get; set; }

        /// <summary>
        /// RabbitMQ通道主题
        /// </summary>
        public string RabbitMQ_EXCHANGE { get; set; } = "";
        public string RabbitMQ_ROUTE { get; set; } = "";
        public string RabbitMQ_QUEUE { get; set; } = "/QinSoft.Core/__EventBus__";

        /// <summary>
        /// 默认编码
        /// </summary>
        public virtual Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        protected CancellationTokenSource SubscribeCancellationToken = new CancellationTokenSource();

        public RabbitMQChannel(IRabbitMQClient client) : this(client, NullLoggerFactory.Instance)
        {

        }

        public RabbitMQChannel(IRabbitMQClient client, ILogger logger) : base(logger)
        {
            ObjectUtils.CheckNull(client, nameof(client));
            this.RabbitMQClient = client;

            this.SubscribeRabbitMQ();
        }

        public RabbitMQChannel(IRabbitMQClient client, ILoggerFactory loggerFactory) : this(client, loggerFactory.CreateLogger<RabbitMQChannel>())
        {

        }

        public RabbitMQChannel(IRabbitMQManager manager, RabbitMQChannelOptions options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public RabbitMQChannel(IRabbitMQManager manager, RabbitMQChannelOptions options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<RabbitMQChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Queue, nameof(options.Queue));
            this.RabbitMQClient = string.IsNullOrEmpty(options.Name) ? manager.GetRabbitMQ() : manager.GetRabbitMQ(options.Name);
            this.RabbitMQ_EXCHANGE = options.Exchange;
            this.RabbitMQ_ROUTE = options.RouteKey;
            this.RabbitMQ_QUEUE = options.Queue;

            this.SubscribeRabbitMQ();
        }

        public RabbitMQChannel(IRabbitMQManager manager, IOptions<RabbitMQChannelOptions> options) : this(manager, options, NullLoggerFactory.Instance)
        {

        }

        public RabbitMQChannel(IRabbitMQManager manager, IOptions<RabbitMQChannelOptions> options, ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<RabbitMQChannel>())
        {
            ObjectUtils.CheckNull(manager, nameof(manager));
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckEmpty(options.Value.Queue, nameof(options.Value.Queue));
            this.RabbitMQClient = string.IsNullOrEmpty(options.Value.Name) ? manager.GetRabbitMQ() : manager.GetRabbitMQ(options.Value.Name);
            this.RabbitMQ_EXCHANGE = options.Value.Exchange;
            this.RabbitMQ_ROUTE = options.Value.RouteKey;
            this.RabbitMQ_QUEUE = options.Value.Queue;

            this.SubscribeRabbitMQ();
        }

        /// <summary>
        /// 订阅RabbitMQ
        /// </summary>
        protected virtual void SubscribeRabbitMQ()
        {
            this.RabbitMQClient.Consume(RabbitMQ_QUEUE, true, (s, e) =>
            {
                this.Read(e.Body.ToArray().ToString(DefaultEncoding).FromJson<ChannelData>());
            }, SubscribeCancellationToken.Token);
        }

        protected override void WriteCore(ChannelData data)
        {
            this.RabbitMQClient.Publish(RabbitMQ_EXCHANGE, RabbitMQ_ROUTE, null, data.ToJson().ToBytes(DefaultEncoding));
        }

        public override void Dispose()
        {
            this.SubscribeCancellationToken.Cancel();
            base.Dispose();
        }
    }

    public class RabbitMQChannelOptions
    {
        public string Name { get; set; }

        public string Exchange { get; set; } = "";

        public string RouteKey { get; set; } = "";

        public string Queue { get; set; } = "QinSoft.Core.__EventBus__";
    }
}
