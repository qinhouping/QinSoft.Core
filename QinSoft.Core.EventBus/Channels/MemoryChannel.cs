using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using QinSoft.Core.MQ.MQTT;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.EventBus.Channels
{
    public class MemoryChannel : Channel
    {
        public MemoryChannel() : base(NullLoggerFactory.Instance.CreateLogger<MemoryChannel>()) { }

        public MemoryChannel(ILogger logger) : base(logger)
        {

        }

        public MemoryChannel(ILoggerFactory loggerFactory) : base(loggerFactory.CreateLogger<MemoryChannel>())
        {

        }

        protected override void WriteCore(ChannelData data)
        {
            this.Read(data);
        }
    }
}
