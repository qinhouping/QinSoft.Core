using QinSoft.Core.MQ.Kafka.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.Kafka
{
    public interface IKafkaManager : IDisposable
    {
        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IKafkaClient<string, string> GetKafka();

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        Task<IKafkaClient<string, string>> GetKafkaAsync();

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IKafkaClient<string, string> GetKafka(string name);

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        Task<IKafkaClient<string, string>> GetKafkaAsync(string name);
    }
}
