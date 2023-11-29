using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QinSoft.Core.EventBus
{
    public interface IPublisher : IDisposable
    {
        bool Publish(ChannelData data);

        bool Publish(string topic, byte[] payload);

        bool Publish(string topic, IDictionary<string, string> headers, byte[] payload);
    }
}
