using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.MQ.MQTT
{
    public class MQTTException : Exception
    {
        public MQTTException(string message) : base(message)
        {

        }

        public MQTTException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
