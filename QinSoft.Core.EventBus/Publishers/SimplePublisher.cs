using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.EventBus.Publishers
{
    public class SimplePublisher : IPublisher
    {
        public Channel channel { get; protected set; }

        public SimplePublisher(Channel channel)
        {
            ObjectUtils.CheckNull(channel, nameof(channel));
            this.channel = channel;
        }

        public bool Publish(ChannelData data)
        {
            ObjectUtils.CheckNull(data, nameof(data));
            ObjectUtils.CheckEmpty(data.Topic, nameof(data.Topic));
            ObjectUtils.CheckEmpty(data.Payload, nameof(data.Payload));
            return this.channel.Write(data);
        }

        public virtual bool Publish(string topic, byte[] payload)
        {
            ObjectUtils.CheckEmpty(topic, nameof(topic));
            ObjectUtils.CheckEmpty(payload, nameof(payload));
            return this.channel.Write(new ChannelData()
            {
                Id = Guid.NewGuid().ToString(),
                Topic = topic,
                Headers = null,
                Payload = payload
            });
        }

        public virtual bool Publish(string topic, IDictionary<string, string> headers, byte[] payload)
        {
            ObjectUtils.CheckEmpty(topic, nameof(topic));
            ObjectUtils.CheckEmpty(payload, nameof(payload));
            return this.channel.Write(new ChannelData()
            {
                Id = Guid.NewGuid().ToString(),
                Topic = topic,
                Headers = headers,
                Payload = payload
            });
        }

        public void Dispose()
        {

        }
    }
}
