using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.InfluxDB
{
    public class InfluxDBException : Exception
    {
        public InfluxDBException(string message) : base(message)
        {

        }

        public InfluxDBException(string message, Exception exception) : base(message, exception)
        {

        }
    }
}
