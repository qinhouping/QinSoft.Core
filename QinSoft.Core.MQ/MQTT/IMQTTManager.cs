using MQTTnet.Client;
using QinSoft.Core.MQ.Kafka.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.MQTT
{
    public interface IMQTTManager : IDisposable
    {
        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        IMqttClient GetMqtt();

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        Task<IMqttClient> GetMqttAsync();

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        IMqttClient GetMqtt(string name);

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        Task<IMqttClient> GetMqttAsync(string name);
    }
}
