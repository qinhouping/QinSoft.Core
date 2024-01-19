using QinSoft.Core.Cache.Local;
using QinSoft.Core.Cache.Local.Core;
using QinSoft.Core.Cache.Redis;
using QinSoft.Core.Cache.Redis.Core;
using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.MQ.Kafka;
using QinSoft.Core.MQ.Kafka.Core;
using System.Threading.Tasks;
using System.Threading;
using Confluent.Kafka;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.MQ.RabbitMQ;
using RabbitMQ.Client;
using QinSoft.Core.MQ.RabbitMQ.Core;
using QinSoft.Core.MQ.MQTT;
using MQTTnet.Client;
using System.Diagnostics;
using MQTTnet.Protocol;
using QinSoft.Core.MQ.MQTT.Core;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class MQUnitTest
    {
        [TestMethod]
        public void TestKafkaManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            KafkaManagerConfig KafkaManagerConfig = configer.Get<KafkaManagerConfig>("KafkaManagerConfig");

            string topic = "qinsoft.kafka.test";

            using (IKafkaManager kafkaManager = new KafkaManager(KafkaManagerConfig))
            {
                ExecuteUtils.ExecuteInThread(() =>
                {
                    IKafkaClient<string, string> client = kafkaManager.GetKafka<string, string>();
                    while (true)
                    {
                        client.ProduceAsync(topic, Guid.NewGuid().ToString(), DateTime.Now.ToString());
                        Thread.Sleep(1000);
                    }
                });

                IKafkaClient<string, string> client = kafkaManager.GetKafka<string, string>();
                client.Consume(topic, (consumer, result) =>
                {
                    Debug.WriteLine("consumer1:\n" + result.ToJson());
                    consumer.Commit(result);
                });

                IKafkaClient<string, string> client2 = kafkaManager.GetKafka<string, string>();
                client2.Consume(topic, (consumer2, result2) =>
                {
                    Debug.WriteLine("consumer2:\n" + result2.Message.Key + ":" + result2.Message.Value);
                    consumer2.Commit(result2);
                });

                Thread.Sleep(1000 * 3600);
            }
        }


        [TestMethod]
        public void TestRabbitMQManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            RabbitMQManagerConfig rabbitMQManagerConfig = configer.Get<RabbitMQManagerConfig>("RabbitMQManagerConfig");

            using (IRabbitMQManager manager = new RabbitMQManager(rabbitMQManagerConfig))
            {
                ExecuteUtils.ExecuteInThread(() =>
                {
                    IRabbitMQClient client = manager.GetRabbitMQ();
                    while (true)
                    {
                        string message = Guid.NewGuid().ToString();
                        client.Publish("amq.direct", "test", null, message);
                        Thread.Sleep(1000);
                    }
                });

                IRabbitMQClient client = manager.GetRabbitMQ();
                client.Consume("test_queue", true, (c, sender, args, msg) =>
                {
                    Debug.WriteLine("consumer1:\n" + msg);
                });

                IRabbitMQClient client2 = manager.GetRabbitMQ();
                client.Consume("test_queue", true, (c, sender, args, msg) =>
                {
                    Debug.WriteLine("consumer2:\n" + msg);
                });

                Thread.Sleep(1000 * 3600);
            }
        }


        [TestMethod]
        public void TestMQTTManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            MQTTManagerConfig mqttManagerConfig = configer.Get<MQTTManagerConfig>("MQTTManagerConfig");

            using (IMQTTManager manager = new MQTTManager(mqttManagerConfig))
            {
                ExecuteUtils.ExecuteInThread(() =>
                {
                    IMQTTClient client = manager.GetMqtt();
                    while (true)
                    {
                        MqttClientPublishResult result = client.PublishAsync("/qinsoft.core/test", "test:" + DateTime.Now.ToString()).Result;
                        Thread.Sleep(1000);
                    }
                });

                IMQTTClient client = manager.GetMqtt();

                client.ApplicationMessageReceivedAsync += async e =>
                {
                    Debug.WriteLine("consumer1:\n" + e.ClientId + "\n" + e.ApplicationMessage.Topic + "\n" + Encoding.Default.GetString(e.ApplicationMessage.Payload));
                    await Task.CompletedTask;
                };
                client.SubscribeAsync("/qinsoft.core/test");

                IMQTTClient client2 = manager.GetMqtt();
                client2.ApplicationMessageReceivedAsync += async e =>
                {
                    Debug.WriteLine("consumer2:\n" + e.ClientId + "\n" + e.ApplicationMessage.Topic + "\n" + Encoding.Default.GetString(e.ApplicationMessage.Payload));
                    await Task.CompletedTask;
                };
                client2.SubscribeAsync("/qinsoft.core/test");

                Thread.Sleep(1000 * 3600);
            }
        }
    }
}
