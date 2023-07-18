using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.Kafka.Core
{
    public class KafkaClient<TKEY, TVALUE> : IKafkaClient<TKEY, TVALUE>
    {
        /// <summary>
        /// Kakfa地址
        /// </summary>
        public string BootstrapServers { get; protected set; }

        /// <summary>
        /// 生产者
        /// </summary>
        public IProducer<TKEY, TVALUE> Producer { get; protected set; }

        private object lockObj = new object();

        public KafkaClient(string bootstrapServers)
        {
            this.BootstrapServers = bootstrapServers;
        }

        /// <summary>
        /// 初始化生产者
        /// </summary>
        protected virtual void InitProducer()
        {
            lock (lockObj)
            {
                ProducerConfig config = new ProducerConfig
                {
                    BootstrapServers = BootstrapServers
                };
                if (Producer == null)
                {
                    Producer = new ProducerBuilder<TKEY, TVALUE>(config).Build();
                }
            }
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Produce(string topic, TKEY key, TVALUE value, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null)
        {
            InitProducer();
            Message<TKEY, TVALUE> msg = new Message<TKEY, TVALUE>() { Key = key, Value = value };
            Producer.Produce(topic, msg, deliveryHandler);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Produce(TopicPartition topicPartition, TKEY key, TVALUE value, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null)
        {
            InitProducer();
            Message<TKEY, TVALUE> msg = new Message<TKEY, TVALUE>() { Key = key, Value = value };
            Producer.Produce(topicPartition, msg, deliveryHandler);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Produce(string topic, Message<TKEY, TVALUE> message, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null)
        {
            InitProducer();
            Producer.Produce(topic, message, deliveryHandler);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Produce(TopicPartition topicPartition, Message<TKEY, TVALUE> message, Action<DeliveryReport<TKEY, TVALUE>> deliveryHandler = null)
        {
            InitProducer();
            Producer.Produce(topicPartition, message, deliveryHandler);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(string topic, TKEY key, TVALUE value, CancellationToken cancellationToken = default)
        {
            InitProducer();
            Message<TKEY, TVALUE> msg = new Message<TKEY, TVALUE>() { Key = key, Value = value };
            return await Producer.ProduceAsync(topic, msg, cancellationToken);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(TopicPartition topicPartition, TKEY key, TVALUE value, CancellationToken cancellationToken = default)
        {
            InitProducer();
            Message<TKEY, TVALUE> msg = new Message<TKEY, TVALUE>() { Key = key, Value = value };
            return await Producer.ProduceAsync(topicPartition, msg, cancellationToken);
        }


        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(string topic, Message<TKEY, TVALUE> message, CancellationToken cancellationToken = default)
        {
            InitProducer();
            return await Producer.ProduceAsync(topic, message, cancellationToken);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task<DeliveryResult<TKEY, TVALUE>> ProduceAsync(TopicPartition topicPartition, Message<TKEY, TVALUE> message, CancellationToken cancellationToken = default)
        {
            InitProducer();
            return await Producer.ProduceAsync(topicPartition, message, cancellationToken);
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        protected virtual IConsumer<TKEY, TVALUE> BuildConsumer(string groupId)
        {
            ConsumerConfig config = new ConsumerConfig()
            {
                BootstrapServers = BootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            return new ConsumerBuilder<TKEY, TVALUE>(config).Build();
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        protected virtual IConsumer<TKEY, TVALUE> BuildConsumer(string groupId, TopicPartition topicPartition)
        {
            ConsumerConfig config = new ConsumerConfig()
            {
                BootstrapServers = BootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            IConsumer<TKEY, TVALUE> consumer = new ConsumerBuilder<TKEY, TVALUE>(config).Build();
            consumer.Assign(topicPartition);
            return consumer;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        protected virtual IConsumer<TKEY, TVALUE> BuildConsumer(string groupId, TopicPartitionOffset topicPartitionOffset)
        {
            ConsumerConfig config = new ConsumerConfig()
            {
                BootstrapServers = BootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
            IConsumer<TKEY, TVALUE> consumer = new ConsumerBuilder<TKEY, TVALUE>(config).Build();
            consumer.Assign(topicPartitionOffset);
            return consumer;
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(string groupId, string topic, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(groupId))
            {
                consumer.Subscribe(topic);
                try
                {
                    while (true)
                    {
                        ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                        resultHandler(result);
                    }
                }
                catch (OperationCanceledException)
                {
                    //取消消费
                }
                finally
                {
                    consumer.Dispose();
                }
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(string groupId, TopicPartition topicPartition, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(groupId, topicPartition))
            {
                consumer.Subscribe(topicPartition.Topic);
                try
                {
                    while (true)
                    {
                        ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                        resultHandler(result);
                    }
                }
                catch (OperationCanceledException)
                {
                    //取消消费
                }
                finally
                {
                    consumer.Dispose();
                }
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(string groupId, TopicPartitionOffset topicPartitionOffset, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(groupId, topicPartitionOffset))
            {
                consumer.Subscribe(topicPartitionOffset.Topic);
                try
                {
                    while (true)
                    {
                        ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                        resultHandler(result);
                    }
                }
                catch (OperationCanceledException)
                {
                    //取消消费
                }
                finally
                {
                    consumer.Dispose();
                }
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(string groupId, string topic, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(groupId))
            {
                consumer.Subscribe(topic);
                try
                {
                    while (true)
                    {
                        ConsumeResult<TKEY, TVALUE> result = await Task.FromResult(consumer.Consume(cancellationToken));
                        resultHandler(result);
                    }
                }
                catch (OperationCanceledException)
                {
                    //取消消费
                }
                finally
                {
                    consumer.Dispose();
                }
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(string groupId, TopicPartition topicPartition, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(groupId, topicPartition))
            {
                consumer.Subscribe(topicPartition.Topic);
                try
                {
                    while (true)
                    {
                        ConsumeResult<TKEY, TVALUE> result = await Task.FromResult(consumer.Consume(cancellationToken));
                        resultHandler(result);
                    }
                }
                catch (OperationCanceledException)
                {
                    //取消消费
                }
                finally
                {
                    consumer.Dispose();
                }
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(string groupId, TopicPartitionOffset topicPartitionOffset, Action<ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(groupId, topicPartitionOffset))
            {
                consumer.Subscribe(topicPartitionOffset.Topic);
                try
                {
                    while (true)
                    {
                        ConsumeResult<TKEY, TVALUE> result = await Task.FromResult(consumer.Consume(cancellationToken));
                        resultHandler(result);
                    }
                }
                catch (OperationCanceledException)
                {
                    //取消消费
                }
                finally
                {
                    consumer.Dispose();
                }
            }
        }

        /// <summary>
        /// 安全资源释放
        /// </summary>
        public virtual void SafeDispose()
        {
            if (Producer != null)
            {
                Producer.Dispose();
            }
        }

        /// <summary>
        /// 资源释放，覆盖原有资源释放防止使用using产生异常
        /// </summary>
        public virtual void Dispose()
        {

        }
    }
}
