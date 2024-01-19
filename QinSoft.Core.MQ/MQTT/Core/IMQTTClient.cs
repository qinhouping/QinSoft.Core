using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.MQTT.Core
{
    public interface IMQTTClient : IDisposable
    {
        event Func<MqttApplicationMessageReceivedEventArgs, Task> ApplicationMessageReceivedAsync;

        bool IsConnected { get; }

        MqttClientConnectResult Connect();

        Task<MqttClientConnectResult> ConnectAsync();

        MqttClientPublishResult Publish(string topic, byte[] data);

        Task<MqttClientPublishResult> PublishAsync(string topic, byte[] data);

        MqttClientPublishResult Publish(string topic, string data);

        Task<MqttClientPublishResult> PublishAsync(string topic, string data);

        MqttClientPublishResult Publish(MqttApplicationMessage message);

        Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage message);

        void Subscribe(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);

        Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);

        void Subscribe(string[] topics, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);

        Task SubscribeAsync(string[] topics, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce);

        void Unsubscribe(string topic);

        Task UnsubscribeAsync(string topic);

        void Unsubscribe(string[] topics);

        Task UnsubscribeAsync(string[] topics);
    }
}
