using Confluent.Kafka;
using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace QinSoft.Core.EventBus.Subscriber
{
    public class SimpleSubscriber : ISubscriber
    {
        protected ConcurrentDictionary<IHandler, ConcurrentDictionary<string, List<MethodInfo>>> HandlersInfo;

        public Channel channel { get; protected set; }

        public SimpleSubscriber(Channel channel)
        {
            ObjectUtils.CheckNull(channel, nameof(channel));
            this.channel = channel;
            this.HandlersInfo = new ConcurrentDictionary<IHandler, ConcurrentDictionary<string, List<MethodInfo>>>();

            this.channel.Readed += Channel_Readed;
        }

        protected virtual void Channel_Readed(object sender, ChannelData e)
        {
            foreach (IHandler handler in HandlersInfo.Keys)
            {
                if (HandlersInfo.TryGetValue(handler, out ConcurrentDictionary<string, List<MethodInfo>> info))
                {
                    if (info.TryGetValue(e.Topic, out List<MethodInfo> minfos))
                    {
                        foreach (MethodInfo mi in minfos)
                        {
                            try
                            {
                                mi.Invoke(handler, new object[] { e });
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
        }

        public virtual bool Subscriber(IHandler handler)
        {
            ObjectUtils.CheckNull(handler, nameof(handler));
            lock (handler)
            {
                //防止重复订阅
                this.Unsubscriber(handler);

                Type type = handler.GetType();
                MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
                foreach (var method in methods)
                {
                    HandlerMethodAttribute attribute = method.GetCustomAttribute<HandlerMethodAttribute>();
                    if (attribute != null)
                    {
                        ConcurrentDictionary<string, List<MethodInfo>> info = this.HandlersInfo.GetOrAdd(handler, key => new ConcurrentDictionary<string, List<MethodInfo>>());

                        foreach (string topic in attribute.Topics)
                        {
                            List<MethodInfo> minfos = info.GetOrAdd(topic, key => new List<MethodInfo>());
                            minfos.Add(method);
                        }
                    }
                }
            }
            return true;
        }

        public virtual bool Unsubscriber(IHandler handler)
        {
            lock (handler)
            {
                return HandlersInfo.TryRemove(handler, out _);
            }
        }

        public virtual void Dispose()
        {
            this.channel.Readed -= Channel_Readed;
        }
    }
}
