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
                    using (IKafkaClient<string, string> kafka = kafkaManager.GetKafka<string, string>())
                    {
                        while (true)
                        {
                            kafka.ProduceAsync(topic, Guid.NewGuid().ToString(), DateTime.Now.ToString());
                            Thread.Sleep(1000);
                        }
                    }
                });
                ExecuteUtils.ExecuteInThread(() =>
                {
                    using (IKafkaClient<string, string> kafka = kafkaManager.GetKafka<string, string>())
                    {
                        kafka.Consume(topic, (consumer, result) =>
                        {
                            Console.WriteLine("consumer1:\n" + result.ToJson());
                            consumer.Commit(result);
                        });
                    }
                });
                ExecuteUtils.ExecuteInThread(() =>
                {
                    using (IKafkaClient<string, string> kafka = kafkaManager.GetKafka<string, string>())
                    {
                        kafka.Consume(topic, (consumer2, result2) =>
                        {
                            Console.WriteLine("consumer2:\n" + result2.Message.Key + ":" + result2.Message.Value);
                            consumer2.Commit(result2);
                        });
                    }
                });

                Thread.Sleep(1000 * 3600);
            }
        }
    }
}
