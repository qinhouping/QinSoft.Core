using Confluent.Kafka;
using QinSoft.Core.Common.Utils;
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
        /// 生产者配置信息
        /// </summary>
        protected ProducerConfig ProducerConfig { get; set; }

        /// <summary>
        /// 消费者配置信息
        /// </summary>
        protected ConsumerConfig ConsumerConfig { get; set; }

        /// <summary>
        /// 键序列化
        /// </summary>
        protected ISerializer<TKEY> KeySerializer { get; set; }

        /// <summary>
        /// 值序列化
        /// </summary>
        protected ISerializer<TVALUE> ValueSerializer { get; set; }

        /// <summary>
        /// 键反序列化
        /// </summary>
        protected IDeserializer<TKEY> KeyDeserializer { get; set; }

        /// <summary>
        /// 值反序列化
        /// </summary>
        protected IDeserializer<TVALUE> ValueDeserializer { get; set; }

        /// <summary>
        /// 生产者
        /// </summary>
        public IProducer<TKEY, TVALUE> Producer { get; protected set; }

        private object lockObj = new object();

        public KafkaClient(ProducerConfig producerConfig) : this(producerConfig, null, null)
        {

        }

        public KafkaClient(ProducerConfig producerConfig, ISerializer<TKEY> keySerializer, ISerializer<TVALUE> valueSerializer)
        {
            this.ProducerConfig = producerConfig;
            this.KeySerializer = keySerializer;
            this.ValueSerializer = valueSerializer;
        }

        public KafkaClient(ConsumerConfig consumerConfig) : this(consumerConfig, null, null)
        {

        }

        public KafkaClient(ConsumerConfig consumerConfig, IDeserializer<TKEY> keyDeserializer, IDeserializer<TVALUE> valueDeserializer)
        {
            this.ConsumerConfig = consumerConfig;
            this.KeyDeserializer = keyDeserializer;
            this.ValueDeserializer = valueDeserializer;
        }

        public KafkaClient(ProducerConfig producerConfig, ConsumerConfig consumerConfig) : this(producerConfig, null, null, consumerConfig, null, null)
        {
        }

        public KafkaClient(ProducerConfig producerConfig, ISerializer<TKEY> keySerializer, ISerializer<TVALUE> valueSerializer, ConsumerConfig consumerConfig, IDeserializer<TKEY> keyDeserializer, IDeserializer<TVALUE> valueDeserializer)
        {
            this.ProducerConfig = producerConfig;
            this.KeySerializer = keySerializer;
            this.ValueSerializer = valueSerializer;
            this.ConsumerConfig = consumerConfig;
            this.KeyDeserializer = keyDeserializer;
            this.ValueDeserializer = valueDeserializer;
        }

        /// <summary>
        /// 初始化生产者
        /// </summary>
        protected virtual void InitProducer()
        {
            lock (lockObj)
            {
                if (Producer == null)
                {
                    ProducerBuilder<TKEY, TVALUE> builder = new ProducerBuilder<TKEY, TVALUE>(this.ProducerConfig);
                    if (KeySerializer != null)
                    {
                        builder.SetKeySerializer(KeySerializer);
                    }
                    if (ValueSerializer != null)
                    {
                        builder.SetValueSerializer(ValueSerializer);
                    }
                    Producer = builder.Build();
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
        protected virtual IConsumer<TKEY, TVALUE> BuildConsumer()
        {
            ConsumerBuilder<TKEY, TVALUE> builder = new ConsumerBuilder<TKEY, TVALUE>(ConsumerConfig);
            if (KeySerializer != null)
            {
                builder.SetKeyDeserializer(KeyDeserializer);
            }
            if (ValueSerializer != null)
            {
                builder.SetValueDeserializer(ValueDeserializer);
            }
            return builder.Build();
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        protected virtual IConsumer<TKEY, TVALUE> BuildConsumer(TopicPartition topicPartition)
        {
            IConsumer<TKEY, TVALUE> consumer = BuildConsumer();
            consumer.Assign(topicPartition);
            return consumer;
        }

        /// <summary>
        /// 创建消费者
        /// </summary>
        protected virtual IConsumer<TKEY, TVALUE> BuildConsumer(TopicPartitionOffset topicPartitionOffset)
        {
            IConsumer<TKEY, TVALUE> consumer = BuildConsumer();
            consumer.Assign(topicPartitionOffset);
            return consumer;
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(string topic, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            ExecuteUtils.ExecuteInThread(() =>
            {
                using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer())
                {
                    consumer.Subscribe(topic);
                    try
                    {
                        while (true)
                        {
                            ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                            resultHandler(consumer, result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //取消消费
                    }
                }
            });
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(TopicPartition topicPartition, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            ExecuteUtils.ExecuteInThread(() =>
            {
                using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(topicPartition))
                {
                    consumer.Subscribe(topicPartition.Topic);
                    try
                    {
                        while (true)
                        {
                            ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                            resultHandler(consumer, result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //取消消费
                    }
                }
            });
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(TopicPartitionOffset topicPartitionOffset, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            ExecuteUtils.ExecuteInThread(() =>
            {
                using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(topicPartitionOffset))
                {
                    consumer.Subscribe(topicPartitionOffset.Topic);
                    try
                    {
                        while (true)
                        {
                            ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                            resultHandler(consumer, result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //取消消费
                    }
                }
            });
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(string topic, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            ExecuteUtils.ExecuteInThread(() =>
            {
                using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer())
                {
                    consumer.Subscribe(topic);
                    try
                    {
                        while (true)
                        {
                            ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                            resultHandler(consumer, result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //取消消费
                    }
                }
            });
            await Task.CompletedTask;
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(TopicPartition topicPartition, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            ExecuteUtils.ExecuteInThread(() =>
            {
                using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(topicPartition))
                {
                    consumer.Subscribe(topicPartition.Topic);
                    try
                    {
                        while (true)
                        {
                            ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                            resultHandler(consumer, result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //取消消费
                    }
                }
            });
            await Task.CompletedTask;
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(TopicPartitionOffset topicPartitionOffset, Action<IConsumer<TKEY, TVALUE>, ConsumeResult<TKEY, TVALUE>> resultHandler, CancellationToken cancellationToken = default)
        {
            ExecuteUtils.ExecuteInThread(() =>
            {
                using (IConsumer<TKEY, TVALUE> consumer = BuildConsumer(topicPartitionOffset))
                {
                    consumer.Subscribe(topicPartitionOffset.Topic);
                    try
                    {
                        while (true)
                        {
                            ConsumeResult<TKEY, TVALUE> result = consumer.Consume(cancellationToken);
                            resultHandler(consumer, result);
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        //取消消费
                    }
                }
            });
            await Task.CompletedTask;
        }

        /// <summary>
        /// 资源释放，覆盖原有资源释放防止使用using产生异常
        /// </summary>
        public virtual void Dispose()
        {
            if (Producer != null)
            {
                Producer.Dispose();
            }
        }
    }
}
