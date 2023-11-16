using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.Kafka.Core
{
    public interface IKafkaClient : IDisposable
    {

    }
    /// <summary>
    /// kafka客户端
    /// </summary>
    public interface IKafkaClient<TKEY, TVALUE> : IKafkaClient
    {
        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Produce(string topic, TKEY key, TVALUE value, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Produce(TopicPartition topicPartition, TKEY key, TVALUE value, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Produce(string topic, Message<TKEY, TVALUE> message, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        void Produce(TopicPartition topicPartition, Message<TKEY, TVALUE> message, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(string topic, TKEY key, TVALUE value, CancellationToken cancellationToken = default);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(TopicPartition topicPartition, TKEY key, TVALUE value, CancellationToken cancellationToken = default);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(string topic, Message<TKEY, TVALUE> message, CancellationToken cancellationToken = default);

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(TopicPartition topicPartition, Message<TKEY, TVALUE> message, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(string topic, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(TopicPartition topicPartition, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(TopicPartitionOffset topicPartitionOffset, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(string topic, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(TopicPartition topicPartition, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(TopicPartitionOffset topicPartitionOffset, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);
    }
}
