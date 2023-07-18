using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.Kafka.Core
{
    /// <summary>
    /// kafka客户端
    /// </summary>
    public interface IKafkaClient<TKEY, TVALUE> : IDisposable
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
        void Consume(string groupId, string topic, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(string groupId, TopicPartition topicPartition, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);


        /// <summary>
        /// 消费者消费队列
        /// </summary>
        void Consume(string groupId, TopicPartitionOffset topicPartitionOffset, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(string groupId, string topic, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(string groupId, TopicPartition topicPartition, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        Task ConsumeAsync(string groupId, TopicPartitionOffset topicPartitionOffset, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default);

        /// <summary>
        /// 安全资源释放
        /// </summary>
        void SafeDispose();
    }
}
