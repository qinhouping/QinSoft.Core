using MQTTnet.Client;
using QinSoft.Core.MQ.Kafka.Core;
using QinSoft.Core.MQ.MQTT.Core;
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
        IMQTTClient GetMqtt();

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        Task<IMQTTClient> GetMqttAsync();

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        IMQTTClient GetMqtt(string name);

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        Task<IMQTTClient> GetMqttAsync(string name);
    }
}
