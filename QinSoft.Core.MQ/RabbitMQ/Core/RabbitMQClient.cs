using Confluent.Kafka;
using QinSoft.Core.Common.Utils;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.RabbitMQ.Core
{
    public class RabbitMQClient : IRabbitMQClient
    {
        /// <summary>
        /// 连接
        /// </summary>
        public virtual IConnection Connection { get; protected set; }

        /// <summary>
        /// 通道（会话）
        /// </summary>
        public virtual IModel Channel { get; protected set; }

        /// <summary>
        /// 默认编码
        /// </summary>
        public virtual Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        public RabbitMQClient(IConnectionFactory connectionFactory, EventHandler<BasicReturnEventArgs> eventHandler=null)
        {
            this.Connection = connectionFactory.CreateConnection();
            this.Channel = this.Connection.CreateModel();
            if (eventHandler != null)
            {
                this.Channel.BasicReturn += eventHandler;
            }
        }

        public RabbitMQClient(IConnection connection, EventHandler<BasicReturnEventArgs> eventHandler = null)
        {
            this.Connection = connection;
            this.Channel = this.Connection.CreateModel();
            if (eventHandler != null)
            {
                this.Channel.BasicReturn += eventHandler;
            }
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Publish(string exchange,string routingKey,IBasicProperties properties,ReadOnlyMemory<byte> body)
        {
            this.Channel.BasicPublish(exchange, routingKey, properties, body);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Publish(PublicationAddress address, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            this.Channel.BasicPublish(address,properties,body);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Publish(string exchange, string routingKey,bool mandatory, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            this.Channel.BasicPublish(exchange, routingKey, mandatory, properties, body);
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Publish(string exchange, string routingKey, IBasicProperties properties, string body)
        {
            this.Channel.BasicPublish(exchange, routingKey, properties, body.ToBytes(this.DefaultEncoding));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Publish(PublicationAddress address, IBasicProperties properties, string body)
        {
            this.Channel.BasicPublish(address, properties, body.ToBytes(this.DefaultEncoding));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual void Publish(string exchange, string routingKey, bool mandatory, IBasicProperties properties, string body)
        {
            this.Channel.BasicPublish(exchange, routingKey, mandatory, properties, body.ToBytes(this.DefaultEncoding));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            await ExecuteUtils.ExecuteInTask(() => this.Channel.BasicPublish(exchange, routingKey, properties, body));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task PublishAsync(PublicationAddress address, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            await ExecuteUtils.ExecuteInTask(() => this.Channel.BasicPublish(address, properties, body));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task PublishAsync(string exchange, string routingKey, bool mandatory, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            await ExecuteUtils.ExecuteInTask(() => this.Channel.BasicPublish(exchange, routingKey, mandatory, properties, body));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task PublishAsync(string exchange, string routingKey, IBasicProperties properties, string body)
        {
            await ExecuteUtils.ExecuteInTask(() => this.Channel.BasicPublish(exchange, routingKey, properties, body.ToBytes(this.DefaultEncoding)));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task PublishAsync(PublicationAddress address, IBasicProperties properties, string body)
        {
            await ExecuteUtils.ExecuteInTask(() => this.Channel.BasicPublish(address, properties, body.ToBytes(this.DefaultEncoding)));
        }

        /// <summary>
        /// 生产者推送队列
        /// </summary>
        public virtual async Task PublishAsync(string exchange, string routingKey, bool mandatory, IBasicProperties properties, string body)
        {
            await ExecuteUtils.ExecuteInTask(() => this.Channel.BasicPublish(exchange, routingKey, mandatory, properties, body.ToBytes(this.DefaultEncoding)));
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(string queue,bool autoAck, EventHandler<BasicDeliverEventArgs> Received, CancellationToken cancellationToken = default)
        {
            // 创建消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);
            consumer.Received += Received;
            string tag= Channel.BasicConsume(queue, autoAck, consumer);
            if (cancellationToken.CanBeCanceled)
            {
                CancellationTokenRegistration registration = default;
                registration = cancellationToken.Register(() =>
                {
                    this.Channel.BasicCancel(tag);
                    registration.Dispose();
                });
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual void Consume(string queue, bool autoAck, Action<IRabbitMQClient, IBasicConsumer, BasicDeliverEventArgs,string> received, CancellationToken cancellationToken = default)
        {
            // 创建消费者
            EventingBasicConsumer consumer = new EventingBasicConsumer(Channel);
            consumer.Received += (sender,args)=>
            {
                received.Invoke(this, (IBasicConsumer)sender, args,args.Body.ToArray().ToString(this.DefaultEncoding));
            };
            string tag = Channel.BasicConsume(queue, autoAck, consumer);
            if (cancellationToken.CanBeCanceled)
            {
                CancellationTokenRegistration registration = default;
                registration = cancellationToken.Register(() =>
                {
                    this.Channel.BasicCancel(tag);
                    registration.Dispose();
                });
            }
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(string queue, bool autoAck, EventHandler<BasicDeliverEventArgs> received, CancellationToken cancellationToken = default)
        {
            await ExecuteUtils.ExecuteInTask(() => Consume(queue, autoAck, received, cancellationToken));
        }

        /// <summary>
        /// 消费者消费队列
        /// </summary>
        public virtual async Task ConsumeAsync(string queue, bool autoAck, Action<IRabbitMQClient, IBasicConsumer, BasicDeliverEventArgs, string> received, CancellationToken cancellationToken = default)
        {
            await ExecuteUtils.ExecuteInTask(() => Consume(queue, autoAck, received, cancellationToken));
        }

        public virtual void Dispose()
        {
            if (Channel != null)
            {
                Channel.Dispose();
            }
            if (Connection != null)
            {
                Connection.Dispose();
            }
        }
    }
}
