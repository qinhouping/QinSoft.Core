using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.MQTT.Core
{
    public class MQTTClient : IMQTTClient
    {
        private IMqttClient mqttClient;
        private object mqttLock = new object();

        public event Func<MqttApplicationMessageReceivedEventArgs, Task> ApplicationMessageReceivedAsync;

        protected MqttClientOptions mqttOptions;

        protected bool autoConnect = true;

        protected CancellationTokenSource reconnectCancellation;

        public virtual bool IsConnected
        {
            get
            {
                return this.mqttClient?.IsConnected == true;
            }
        }

        public MQTTClient(MqttClientOptions options, bool autoConnect = true, bool autoReconnect = true)
        {
            this.mqttOptions = options;
            if (autoReconnect)
            {
                AutoReconnect();
            }
        }

        protected virtual void AutoReconnect()
        {
            this.reconnectCancellation = new CancellationTokenSource();
            CancellationToken reconnectCancellationToken = this.reconnectCancellation.Token;
            new Thread(() =>
            {
                while (!reconnectCancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        lock (mqttLock)
                        {
                            if (mqttClient != null && !mqttClient.IsConnected)
                            {
                                mqttClient.ConnectAsync(this.mqttOptions).Wait();
                            }
                        }
                    }
                    catch
                    {

                    }
                    finally
                    {
                        Thread.Sleep(1000);
                    }
                }
            }).Start();
        }

        protected virtual IMqttClient GetMqttClient()
        {
            lock (mqttLock)
            {
                if (mqttClient == null)
                {
                    mqttClient = new MqttFactory().CreateMqttClient();
                    mqttClient.ApplicationMessageReceivedAsync += MqttClient_ApplicationMessageReceivedAsync;
                }
                if (autoConnect && !mqttClient.IsConnected)
                {
                    mqttClient.ConnectAsync(this.mqttOptions).Wait();
                }
                return mqttClient;
            }
        }

        protected async Task MqttClient_ApplicationMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs arg)
        {
            await this.ApplicationMessageReceivedAsync(arg);
        }

        public virtual MqttClientConnectResult Connect()
        {
            return this.GetMqttClient().ConnectAsync(mqttOptions).Result;
        }

        public virtual async Task<MqttClientConnectResult> ConnectAsync()
        {
            return await this.GetMqttClient().ConnectAsync(mqttOptions);
        }

        public virtual MqttClientPublishResult Publish(string topic, byte[] data)
        {
            return this.GetMqttClient().PublishBinaryAsync(topic, data).Result;
        }

        public virtual async Task<MqttClientPublishResult> PublishAsync(string topic, byte[] data)
        {
            return await this.GetMqttClient().PublishBinaryAsync(topic, data);
        }

        public virtual MqttClientPublishResult Publish(string topic, string data)
        {
            return this.GetMqttClient().PublishStringAsync(topic, data).Result;
        }

        public virtual async Task<MqttClientPublishResult> PublishAsync(string topic, string data)
        {
            return await this.GetMqttClient().PublishStringAsync(topic, data);
        }

        public virtual MqttClientPublishResult Publish(MqttApplicationMessage message)
        {
            return this.GetMqttClient().PublishAsync(message).Result;
        }

        public virtual async Task<MqttClientPublishResult> PublishAsync(MqttApplicationMessage message)
        {
            return await this.GetMqttClient().PublishAsync(message);
        }

        public virtual void Subscribe(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            this.GetMqttClient().SubscribeAsync(topic, qos).Wait();
        }

        public virtual async Task SubscribeAsync(string topic, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            await this.GetMqttClient().SubscribeAsync(topic, qos);
        }

        public virtual void Subscribe(string[] topics, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            foreach (string topic in topics)
            {
                Subscribe(topic, qos);
            }
        }

        public virtual async Task SubscribeAsync(string[] topics, MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
        {
            foreach (string topic in topics)
            {
                await SubscribeAsync(topic, qos);
            }
        }

        public virtual void Unsubscribe(string topic)
        {
            this.GetMqttClient().UnsubscribeAsync(topic).Wait();
        }

        public virtual async Task UnsubscribeAsync(string topic)
        {
            await this.GetMqttClient().UnsubscribeAsync(topic);
        }

        public virtual void Unsubscribe(string[] topics)
        {
            foreach (string topic in topics)
            {
                Unsubscribe(topic);
            }
        }

        public virtual async Task UnsubscribeAsync(string[] topics)
        {
            foreach (string topic in topics)
            {
                await UnsubscribeAsync(topic);
            }
        }

        public void Dispose()
        {
            reconnectCancellation?.Cancel();
            if (mqttClient != null)
            {
                mqttClient.ApplicationMessageReceivedAsync -= MqttClient_ApplicationMessageReceivedAsync;
                mqttClient.Dispose();
            }
        }
    }
}
