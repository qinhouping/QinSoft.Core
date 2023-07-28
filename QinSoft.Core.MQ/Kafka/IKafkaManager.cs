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
        /// 获取kafka客户端
        /// </summary>
        IKafkaClient<TKEY, TVALUE> GetKafka<TKEY, TVALUE>();

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        Task<IKafkaClient<TKEY, TVALUE>> GetKafkaAsync<TKEY, TVALUE>();

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        IKafkaClient<TKEY, TVALUE> GetKafka<TKEY, TVALUE>(string name);

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        Task<IKafkaClient<TKEY, TVALUE>> GetKafkaAsync<TKEY, TVALUE>(string name);
    }
}
