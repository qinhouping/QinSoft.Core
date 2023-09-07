using QinSoft.Core.MQ.Kafka.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using QinSoft.Core.MQ.RabbitMQ.Core;

namespace QinSoft.Core.MQ.RabbitMQ
{
    public interface IRabbitMQManager : IDisposable
    {
        /// <summary>
        /// 获取rabbitmq客户端
        /// </summary>
        IRabbitMQClient GetRabbitMQ();

        /// <summary>
        /// 获取rabbitmq客户端
        /// </summary>
        Task<IRabbitMQClient> GetRabbitMQAsync();

        /// <summary>
        /// 获取rabbitmq客户端
        /// </summary>
        IRabbitMQClient GetRabbitMQ(string name);

        /// <summary>
        /// 获取rabbitmq客户端
        /// </summary>
        Task<IRabbitMQClient> GetRabbitMQAsync(string name);
    }
}
