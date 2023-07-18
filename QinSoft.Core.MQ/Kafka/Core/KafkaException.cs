using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.MQ.Kafka.Core
{
    public class KafkaException : Exception
    {
        public KafkaException(string message) : base(message)
        {

        }

        public KafkaException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
