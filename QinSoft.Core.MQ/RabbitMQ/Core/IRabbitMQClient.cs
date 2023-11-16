using Confluent.Kafka;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.RabbitMQ.Core
{

    /// <summary>
    /// rabbitmq客户端
    /// </summary>
    public interface IRabbitMQClient:IDisposable
    {
        /// <summary>
        /// 连接
        /// </summary>
        IConnection Connection { get;}

        /// <summary>
        /// 通道
        /// </summary>
        IModel Channel { get; }

        /// <summary>
        /// 默认编码
        /// </summary>
        Encoding DefaultEncoding { get; set; }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Publish(string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Publish(PublicationAddress address, IBasicProperties properties, ReadOnlyMemory<byte> body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Publish(string exchange, string routingKey, bool mandatory, IBasicProperties properties, ReadOnlyMemory<byte> body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Publish(string exchange, string routingKey, IBasicProperties properties, string body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Publish(PublicationAddress address, IBasicProperties properties, string body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Publish(string exchange, string routingKey, bool mandatory, IBasicProperties properties, string body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task PublishAsync(PublicationAddress address, IBasicProperties properties, ReadOnlyMemory<byte> body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task PublishAsync(string exchange, string routingKey, bool mandatory, IBasicProperties properties, ReadOnlyMemory<byte> body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, string body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task PublishAsync(PublicationAddress address, IBasicProperties properties, string body);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task PublishAsync(string exchange, string routingKey, bool mandatory, IBasicProperties properties, string body);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(string queue, bool autoAck, EventHandler<BasicDeliverEventArgs> received, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(string queue, bool autoAck, Action<IRabbitMQClient, IBasicConsumer, BasicDeliverEventArgs, string> received, CancellationToken cancellationToken = default);
      
        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(string queue, bool autoAck, EventHandler<BasicDeliverEventArgs> received, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(string queue, bool autoAck, Action<IRabbitMQClient, IBasicConsumer, BasicDeliverEventArgs, string> received, CancellationToken cancellationToken = default);
    }
}
