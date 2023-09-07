using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.MQ.RabbitMQ.Core
{
    public class RabbitMQException : Exception
    {
        public RabbitMQException(string message) : base(message)
        {

        }

        public RabbitMQException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
