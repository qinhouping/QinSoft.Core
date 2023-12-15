using Microsoft.VisualStudio.TestTools.UnitTesting;
using QinSoft.Core.EventBus;
using QinSoft.Core.EventBus.Channels;
using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet.Client;
using System.Threading;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class EventBusUnitTest
    {
        [TestMethod]
        public void TestMemoryChannel()
        {
            Channel channel = new MemoryChannel();
            ExecuteUtils.ExecuteInThread(() =>
            {
                while (true)
                {
                    channel.Write(new ChannelData()
                    {
                        Topic = "test",
                        Payload = DateTime.Now.ToString().ToBytes()
                    });
                    Thread.Sleep(1000);
                }
            });

            channel.Readed += Channel_Readed;

            Thread.Sleep(1000 * 3600);
        }

        [TestMethod]
        public void TestMQTTChannel()
        {
            Channel channel = Programe.ServiceProvider.GetService<Channel>();
            ExecuteUtils.ExecuteInThread(() =>
            {
                while (true)
                {
                    channel.Write(new ChannelData()
                    {
                        Topic = "test",
                        Payload = DateTime.Now.ToString().ToBytes()
                    });
                    Thread.Sleep(1000);
                }
            });

            channel.Readed += Channel_Readed;

            Thread.Sleep(1000 * 3600);
        }

        [TestMethod]
        public void TestEventBus()
        {
            IPublisher publisher = Programe.ServiceProvider.GetService<IPublisher>();
            ISubscriber subscriber = Programe.ServiceProvider.GetService<ISubscriber>();
            subscriber.Subscriber(new TestEventBusHandler());
            ExecuteUtils.ExecuteInThread(() =>
            {
                while (true)
                {
                    publisher.Publish("test", DateTime.Now.ToString().ToBytes());
                    Thread.Sleep(1000);
                }
            });

            Thread.Sleep(1000 * 3600);
        }

        private void Channel_Readed(object sender, ChannelData e)
        {
            Debug.WriteLine(e.Topic + ":" + BaseUtils.ToString(e.Payload));
        }
    }

    class TestEventBusHandler : IHandler
    {
        [HandlerMethod("test")]
        public void Work(ChannelData data)
        {
            Debug.WriteLine(data.Topic + ":" + BaseUtils.ToString(data.Payload));
        }
    }
}
