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

namespace QinSoft.Core.Test
{
    [TestClass]
    public class MQUnitTest
    {
        [TestMethod]
        public void TestKafka()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            KafkaManagerConfig KafkaManagerConfig = configer.Get<KafkaManagerConfig>("KafkaManagerConfig");

            string topic = "test2";

            using (IKafkaManager kafkaManager = new KafkaManager(KafkaManagerConfig))
            {
                Task.Factory.StartNew(() =>
                {
                    using (IKafkaClient<string, string> kafka = kafkaManager.GetKafka<string, string>())
                    {
                        while (true)
                        {
                            kafka.ProduceAsync(topic, DateTime.Now.Ticks.ToString(), DateTime.Now.ToString());
                            Thread.Sleep(1000);
                        }
                    }
                });
                Task.Factory.StartNew(() =>
                {
                    using (IKafkaClient<string, string> kafka = kafkaManager.GetKafka<string, string>())
                    {
                        kafka.Consume(topic, (consumer, result) =>
                        {
                            Console.WriteLine(result.Message.Key + ":" + result.Message.Value);
                        });
                    }
                });
                Task.Factory.StartNew(() =>
                {
                    using (IKafkaClient<string, string> kafka = kafkaManager.GetKafka<string, string>())
                    {
                        kafka.Consume(topic, (consumer2, result2) =>
                        {
                            Console.WriteLine(result2.Message.Key + ":" + result2.Message.Value);
                        });
                    }
                });

                Thread.Sleep(10000);
            }
        }
    }
}
