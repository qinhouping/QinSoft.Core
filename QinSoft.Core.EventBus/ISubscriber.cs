using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.EventBus
{
    public interface ISubscriber : IDisposable
    {
        bool Subscriber(IHandler handler);

        bool Unsubscriber(IHandler handler);
    }
}
